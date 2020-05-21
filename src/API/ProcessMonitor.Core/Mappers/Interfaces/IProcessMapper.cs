using System.Diagnostics;
using ProcessMonitor.Core.DTOs;

namespace ProcessMonitor.Core.Mappers.Interfaces
{
    public interface IProcessMapper
    {
        bool TryMapToDto(Process process, out ProcessDto processDto);
    }
}
