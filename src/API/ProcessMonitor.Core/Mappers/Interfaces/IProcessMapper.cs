using System.Diagnostics;
using ProcessMonitor.Core.DTOs;

namespace ProcessMonitor.Core.Mappers.Interfaces
{
    public interface IProcessMapper
    {
        ProcessDto MapToDto(Process process);
    }
}
