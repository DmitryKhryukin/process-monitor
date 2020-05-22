using ProcessMonitor.Core.DTOs;

namespace ProcessMonitor.Core.Services.Interfaces
{
    public interface ISystemHealthInfoService
    {
        SystemHealthInfoDto GetSystemHealthInfo();
    }
}
