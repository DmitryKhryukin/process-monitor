using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ProcessMonitor.Core.DTOs;
using ProcessMonitor.Core.Services.Interfaces;

namespace ProcessMonitor.Core.Services
{
    public class ProcessService : IProcessService
    {
        public IEnumerable<ProcessDto> GetCurrentProcesses()
        {
            var processes = Process.GetProcesses();

            return processes.Select(x => new ProcessDto()
            {
                Name = x.ProcessName
            });
        }
    }
}
