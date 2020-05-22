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

        /// <summary>
        /// have to use ProcessMapper.TryMapToDto method
        /// because of .NET Core issue https://github.com/dotnet/runtime/issues/36777
        /// </summary>
        /// <returns></returns>
        public SystemHealthInfoDto GetSystemHealthInfo()
        {
            var result = new SystemHealthInfoDto();

            var processes = Process.GetProcesses();

            var mappedProcesses = new List<ProcessDto>();

            foreach (var process in processes)
            {
                if (_processMapper.TryMapToDto(process, out ProcessDto processDto))
                {
                    mappedProcesses.Add(processDto);
                }
            }

            result.Processes = mappedProcesses.OrderBy(x => x.ProcessName).ToList();


            return result;
        }

        #region Not Used

        // can't get an access to TotalProcessorTime because of the following issue
        // https://github.com/dotnet/runtime/issues/36777

        private async Task<double> GetTotalCpuLoadAsync(int cpuMeasurementWindowSec,
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

        #endregion
    }
}
