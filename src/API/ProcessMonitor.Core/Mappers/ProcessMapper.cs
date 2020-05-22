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
                var message = $"Process Mapping error. ProcessName: {process.ProcessName}; ProcessId: {process.Id}; Error Message:{e.Message}";
                _logger.LogWarning(message);
            }

            return result;
        }

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
                State = process.Responding ? "running" : "sleeping",
            };
        }
    }
}
