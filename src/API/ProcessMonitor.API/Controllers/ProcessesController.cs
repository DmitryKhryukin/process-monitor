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

        private const int DelaySec = 3;

        public ProcessesController(ISystemHealthInfoService systemHealthInfoService, ILogger<ProcessesController> logger)
        {
            _systemHealthInfoService = systemHealthInfoService;
            _logger = logger;
        }

        [HttpGet]
        public async Task GetProcessesAsync(CancellationToken cancellationToken)
        {
            Response.Headers.Add("Content-Type", "text/event-stream");

            _logger.LogInformation("Connection open");

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(DelaySec));
                var systemHealthInfo = _systemHealthInfoService.GetSystemHealthInfo();

                string jsonCustomer = JsonSerializer.Serialize(systemHealthInfo);
                string data = $"data: {jsonCustomer}\n\n";
                await Response.WriteAsync(data);
                await Response.Body.FlushAsync();
            }

            _logger.LogInformation("Connection closed");
        }
    }
}
