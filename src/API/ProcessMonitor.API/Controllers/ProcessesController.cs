using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using Microsoft.AspNetCore.Http;
using ProcessMonitor.Core.Services.Interfaces;

namespace ProcessMonitor.API.Controllers
{
    [ApiController]
    [Route("api/processes/")]
    public class ProcessesController : ControllerBase
    {
        private readonly ILogger<ProcessesController> _logger;
        private readonly ISystemHealthInfoService _systemHealthInfoService;

        public ProcessesController(ISystemHealthInfoService systemHealthInfoService, ILogger<ProcessesController> logger)
        {
            _systemHealthInfoService = systemHealthInfoService;
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

                await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);
                var processes = await _systemHealthInfoService.GetCurrentProcesses();

                string jsonCustomer = JsonSerializer.Serialize(processes);
                string data = $"data: {jsonCustomer}\n\n";
                await Response.WriteAsync(data, cancellationToken: cancellationToken);
                await Response.Body.FlushAsync(cancellationToken);
            }
        }
    }
}
