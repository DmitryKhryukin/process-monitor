using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using ProcessMonitor.Core.DTOs;
using ProcessMonitor.Core.Mappers.Interfaces;

namespace ProcessMonitor.Core.Mappers
{
    public class ProcessMapper : IProcessMapper
    {
        private readonly ILogger<ProcessMapper> _logger;

        public ProcessMapper(ILogger<ProcessMapper> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// it's a bug related to TotalProcessorTime, UserProcessorTime and PrivilegedProcessorTime
        /// check the links above
        /// </summary>
        /// <param name="process"></param>
        /// <param name="processDto"></param>
        /// <returns></returns>
        public bool TryMapToDto(Process process, out ProcessDto processDto)
        {
            var result = true;

            try
            {
                processDto = MapToDto(process);
            }
            catch(Exception e)
            {
                result = false;
                processDto = null;

                var message = $"ProcessName: {process.ProcessName}; ProcessId: {process.Id}; Error Message:{e.Message}";
                _logger.Log(LogLevel.Error, message);
            }

            return result;
        }

        /// <summary>
        /// https://github.com/dotnet/runtime/issues/36777
        /// https://stackoverflow.com/questions/61921540/process-prototalprocessortime-throws-invalidoperationexception-exception-on-maco
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        private ProcessDto MapToDto(Process process)
        {
            return new ProcessDto()
            {
                Id = process.Id,
                ProcessName = string.IsNullOrWhiteSpace(process.ProcessName) ? $"Id : {process.Id}" : process.ProcessName,
                ThreadsCount = process.Threads.Count,
                PhysicalMemoryUsage = process.WorkingSet64,
                UserProcessorTime  = process.UserProcessorTime.TotalMilliseconds,
                PrivilegedProcessorTime = process.PrivilegedProcessorTime.TotalMilliseconds,
                TotalProcessorTime = process.TotalProcessorTime.TotalMilliseconds,
                PagedSystemMemorySize = process.PagedSystemMemorySize64,
                PagedMemorySize = process.PagedMemorySize64,
                State = process.Responding ? "running" : "sleeping",
            };
        }
    }
}
