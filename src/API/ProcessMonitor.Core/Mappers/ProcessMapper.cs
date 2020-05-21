using System.Diagnostics;
using ProcessMonitor.Core.DTOs;
using ProcessMonitor.Core.Mappers.Interfaces;

namespace ProcessMonitor.Core.Mappers
{
    public class ProcessMapper : IProcessMapper
    {
        public ProcessDto MapToDto(Process process)
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
