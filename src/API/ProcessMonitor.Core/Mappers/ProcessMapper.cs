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
                Id = process.Id,
                ProcessName = process.ProcessName,
                ThreadsCount = process.Threads.Count,
                // it's a bug related to TotalProcessorTime, so can't use it for now - check link above
                CpuTime = 1000 //x.TotalProcessorTime.TotalMilliseconds
            };
        }
    }
}
