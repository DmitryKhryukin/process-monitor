using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ProcessMonitor.Core.DTOs;
using ProcessMonitor.Core.Mappers.Interfaces;
using ProcessMonitor.Core.Services.Interfaces;

namespace ProcessMonitor.Core.Services
{
    /// <summary>
    /// it's a .NET Core bug related to TotalProcessorTime, UserProcessorTime and PrivilegedProcessorTime
    /// https://github.com/dotnet/runtime/issues/36777
    /// </summary>
    public class SystemHealthInfoService : ISystemHealthInfoService
    {
        private readonly IProcessMapper _processMapper;
        private readonly ILogger<SystemHealthInfoService> _logger;

        public SystemHealthInfoService(IProcessMapper processMapper,
            ILogger<SystemHealthInfoService> logger)
        {
            _processMapper = processMapper;
            _logger = logger;
        }

        public async Task<SystemHealthInfoDto> GetSystemHealthInfo(int cpuMeasurementWindow, CancellationToken cancellationToken)
        {
            var result = new SystemHealthInfoDto();

            var processes = Process.GetProcesses();

            var verifiableProcesses = GetVerifiableProcesses(processes);
            var cpuLoad = await GetCpuLoadAsync(cpuMeasurementWindow,
                verifiableProcesses,
                cancellationToken);

            return new SystemHealthInfoDto()
            {
                CpuLoad = cpuLoad,
                Processes = verifiableProcesses
                                                .Select(_processMapper.MapToDto)
                                                .OrderBy(x => x.ProcessName)
                                                .ToList()
            };
        }

        /// <summary>
        /// have to use this method
        /// because it's a .NET Core bug related to TotalProcessorTime, UserProcessorTime and PrivilegedProcessorTime
        /// https://github.com/dotnet/runtime/issues/36777
        /// </summary>
        /// <param name="processes"></param>
        /// <returns></returns>
        private IEnumerable<Process> GetVerifiableProcesses(Process[] processes)
        {
            var result = new List<Process>();

            foreach (var process in processes)
            {
                try
                {
                    var totalProcessTime = process.TotalProcessorTime;
                    result.Add(process);
                }
                catch (Exception e)
                {
                    //var message = $"ProcessName: {process.ProcessName}; ProcessId: {process.Id}; Error Message:{e.Message}";
                    //_logger.Log(LogLevel.Warning, message);
                }
            }

            return result;
        }

        private async Task<double> GetCpuLoadAsync(int cpuMeasurementWindowSec,
            IEnumerable<Process> processes,
            CancellationToken cancellationToken)
        {
            TimeSpan startCpuUsage = GetTotalProcessorTime(processes);
            Stopwatch timer = Stopwatch.StartNew();

            await Task.Delay(TimeSpan.FromSeconds(cpuMeasurementWindowSec), cancellationToken);

            TimeSpan endCpuUsage = GetTotalProcessorTime(processes);
            timer.Stop();

            return 100 * (endCpuUsage - startCpuUsage).TotalMilliseconds /
                   (Environment.ProcessorCount * timer.ElapsedMilliseconds);
        }

        private TimeSpan GetTotalProcessorTime(IEnumerable<Process> processes)
        {
            return new TimeSpan(processes.Sum(r => r.TotalProcessorTime.Ticks));
        }
    }
}
