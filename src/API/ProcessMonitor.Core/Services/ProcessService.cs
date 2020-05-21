using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public IEnumerable<ProcessDto> GetCurrentProcesses()
        {
            var processes = Process.GetProcesses();

            var verifiableProcesses = GetVerifiableProcesses(processes);

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
                    var message = $"ProcessName: {process.ProcessName}; ProcessId: {process.Id}; Error Message:{e.Message}";
                    _logger.Log(LogLevel.Warning, message);
                }
            }

            return result;
        }
    }
}
