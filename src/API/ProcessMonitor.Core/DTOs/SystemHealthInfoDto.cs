using System.Collections.Generic;

namespace ProcessMonitor.Core.DTOs
{
    public class SystemHealthInfoDto
    {
        public double CpuLoad { get; set; }
        public IEnumerable<ProcessDto> Processes { get; set; }
    }
}
