using System;

namespace ProcessMonitor.Core.DTOs
{
    public class ProcessDto
    {
        public int Id { get; set; }
        public string ProcessName { get; set; }
        public double CpuTime { get; set; }
        public int ThreadsCount { get; set; }
    }
}
