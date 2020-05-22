using System.Collections.Generic;

namespace ProcessMonitor.Core.DTOs
{
    public class SystemHealthInfoDto
    {
        public SystemHealthInfoDto()
        {
            Processes = new List<ProcessDto>();
        }

        public List<ProcessDto> Processes { get; set; }
    }
}
