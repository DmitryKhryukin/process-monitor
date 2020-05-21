using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    public class ProcessService : IProcessService
    {
        private readonly IProcessMapper _processMapper;
        private readonly ILogger<ProcessService> _logger;

        public ProcessService(IProcessMapper processMapper,
            ILogger<ProcessService> logger)
        {
            _processMapper = processMapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProcessDto>> GetCurrentProcesses()
        {
            var processes = Process.GetProcesses();

            var verifiableProcesses = GetVerifiableProcesses(processes);
            var cpuLoad = await GetCpuLoadAsync(TimeSpan.FromSeconds(1), verifiableProcesses);

            _logger.Log(LogLevel.Information, $"CPU load = {cpuLoad}");

            return verifiableProcesses.Select(_processMapper.MapToDto).OrderBy(x => x.ProcessName).ToList();
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
                catch(Exception e)
                {
                    //var message = $"ProcessName: {process.ProcessName}; ProcessId: {process.Id}; Error Message:{e.Message}";
                   //_logger.Log(LogLevel.Warning, message);
                }
            }

            return result;
        }

        private async Task<double> GetCpuLoadAsync(TimeSpan MeasurementWindow, IEnumerable<Process> processes)
        {
            TimeSpan startCpuUsage = GetTotalProcessorTime(processes);
            Stopwatch timer = Stopwatch.StartNew();

            await Task.Delay(MeasurementWindow);

            TimeSpan endCpuUsage = GetTotalProcessorTime(processes);
            timer.Stop();

            return 100*(endCpuUsage - startCpuUsage).TotalMilliseconds / (Environment.ProcessorCount * timer.ElapsedMilliseconds);
        }

        private TimeSpan GetTotalProcessorTime(IEnumerable<Process> processes)
        {
            return new TimeSpan(processes.Sum(r => r.TotalProcessorTime.Ticks));
        }
    }
}
