using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProcessMonitor.Core.DTOs;

namespace ProcessMonitor.Core.Services.Interfaces
{
    public interface ISystemHealthInfoService
    {
        SystemHealthInfoDto GetSystemHealthInfo();
    }
}
