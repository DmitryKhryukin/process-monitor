using System.Collections.Generic;
using ProcessMonitor.Core.DTOs;

namespace ProcessMonitor.Core.Services.Interfaces
{
    public interface IProcessService
    {
        IEnumerable<ProcessDto> GetCurrentProcesses();
    }
}
