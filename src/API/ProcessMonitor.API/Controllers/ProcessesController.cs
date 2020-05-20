using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace ProcessMonitor.API.Controllers
{
    [ApiController]
    [Route("api/processes/")]
    public class ProcessesController : ControllerBase
    {
        private readonly ILogger<ProcessesController> _logger;

        public ProcessesController(ILogger<ProcessesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task GetProcessesAsync(CancellationToken cancellationToken)
        {
            Response.Headers.Add("Content-Type", "text/event-stream");

            _logger.Log(LogLevel.Information, "Connection open");

            while (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Update");
                _logger.Log(LogLevel.Information, "Get processes");

                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                string dataItem = DateTime.Now.ToString(CultureInfo.InvariantCulture);

                byte[] dataItemBytes = Encoding.ASCII.GetBytes(dataItem);
                await Response.Body.WriteAsync(dataItemBytes,0,dataItemBytes.Length, cancellationToken);
                await Response.Body.FlushAsync(cancellationToken);
            }

            _logger.Log(LogLevel.Information, "Connection closed");
           // Console.WriteLine("Connection closed");
        }
    }
}
