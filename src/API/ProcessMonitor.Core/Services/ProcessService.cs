using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ProcessMonitor.Core.DTOs;
using ProcessMonitor.Core.Mappers;
using ProcessMonitor.Core.Mappers.Interfaces;
using ProcessMonitor.Core.Services.Interfaces;

namespace ProcessMonitor.Core.Services
{
    public class ProcessService : IProcessService
    {
        private readonly IProcessMapper _processMapper;

        public ProcessService(IProcessMapper processMapper)
        {
            _processMapper = processMapper;
        }

        public IEnumerable<ProcessDto> GetCurrentProcesses()
        {
            var processes = Process.GetProcesses();

            var result = new List<ProcessDto>();

            Parallel.ForEach(processes, process =>
            {
                if (_processMapper.TryMapToDto(process, out ProcessDto processDto))
                {
                    result.Add(processDto);
                }
            });

            return result.OrderBy(x => x.ProcessName).ToList();
        }
    }
}
