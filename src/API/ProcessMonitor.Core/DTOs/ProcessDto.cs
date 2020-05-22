namespace ProcessMonitor.Core.DTOs
{
    public class ProcessDto
    {
        public int Id { get; set; }
        public string ProcessName { get; set; }
        public double TotalProcessorTime { get; set; }
        public int ThreadsCount { get; set; }
        public long PhysicalMemoryUsage { get; set; }
        public double UserProcessorTime { get; set; }
        public double PrivilegedProcessorTime { get; set; }
        public string State { get; set; }
    }
}
