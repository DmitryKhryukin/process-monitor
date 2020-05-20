using System.Diagnostics;
using ProcessMonitor.Core.DTOs;

namespace ProcessMonitor.Core.Mappers
{
    public class ProcessMapper
    {
        /// <summary>
        /// it's a bug related to TotalProcessorTime, so can't use it for now
        /// https://github.com/dotnet/runtime/issues/36777
        /// https://stackoverflow.com/questions/61921540/process-prototalprocessortime-throws-invalidoperationexception-exception-on-maco
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static ProcessDto MapToDto(Process process)
        {
            return new ProcessDto()
            {
                // it's a bug related to TotalProcessorTime, UserProcessorTime and PrivilegedProcessorTime, so can't use it for now - check link above
                Id = process.Id,
                ProcessName = string.IsNullOrWhiteSpace(process.ProcessName) ? $"Id : {process.Id}" : process.ProcessName,
                ThreadsCount = process.Threads.Count,
                PhysicalMemoryUsage = process.WorkingSet64,
                //UserProcessorTime  = process.UserProcessorTime.TotalMilliseconds,
                //PrivilegedProcessorTime = process.PrivilegedProcessorTime.TotalMilliseconds,
                //TotalProcessorTime = 1000, //x.TotalProcessorTime.TotalMilliseconds
                PagedSystemMemorySize = process.PagedSystemMemorySize64,
                PagedMemorySize = process.PagedMemorySize64,
                State = process.Responding ? "running" : "sleeping",
            };
        }
    }
}
