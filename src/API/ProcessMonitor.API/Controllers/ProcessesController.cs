using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using ProcessMonitor.Core.Services.Interfaces;

namespace ProcessMonitor.API.Controllers
{
    [ApiController]
    [Route("api/processes/")]
    public class ProcessesController : ControllerBase
    {
        private readonly ILogger<ProcessesController> _logger;
        private readonly IProcessService _processService;

        public ProcessesController(IProcessService processService, ILogger<ProcessesController> logger)
        {
            _processService = processService;
            _logger = logger;
        }

        [HttpGet]
        public async Task GetProcessesAsync(CancellationToken cancellationToken)
        {
            Response.Headers.Add("Content-Type", "text/event-stream");

            _logger.Log(LogLevel.Information, "Connection open");

            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.Log(LogLevel.Information, "Get processes");

                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                var processes = _processService.GetCurrentProcesses();

                var jsonString = JsonSerializer.Serialize(processes);
                byte[] dataItemBytes = Encoding.ASCII.GetBytes(jsonString);

                await Response.Body.WriteAsync(dataItemBytes,0,dataItemBytes.Length, cancellationToken);
                await Response.Body.FlushAsync(cancellationToken);
            }

            _logger.Log(LogLevel.Information, "Connection closed");
        }
    }
}
