using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ProcessMonitor.Core.DTOs;
using ProcessMonitor.Core.Services.Interfaces;

namespace ProcessMonitor.Core.Services
{
    public class ProcessService : IProcessService
    {
        /// <summary>
        /// it's a bug related to TotalProcessorTime, so can't use it for now
        /// https://github.com/dotnet/runtime/issues/36777
        /// https://stackoverflow.com/questions/61921540/process-prototalprocessortime-throws-invalidoperationexception-exception-on-maco
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProcessDto> GetCurrentProcesses()
        {
            var processes = Process.GetProcesses();

            return processes
                .Select(x => new ProcessDto()
            {
                Id = x.Id,
                ProcessName = x.ProcessName,
                ThreadsCount = x.Threads.Count,
                // it's a bug related to TotalProcessorTime, so can't use it for now - check link above
                 CpuTime = 1000 //x.TotalProcessorTime.TotalMilliseconds
            }).ToList();
        }
    }
}
