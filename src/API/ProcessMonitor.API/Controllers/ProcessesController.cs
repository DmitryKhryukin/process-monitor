using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
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
            string[] data = new string[] {
                "Hello World!",
                "Hello Galaxy!",
                "Hello Universe!"
            };

            Response.Headers.Add("Content-Type", "text/event-stream");

            for (int i = 0; i < data.Length; i++)
            {
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                string dataItem = $"data: {data[i]}\n\n";

                byte[] dataItemBytes = Encoding.ASCII.GetBytes(dataItem);
                await Response.Body.WriteAsync(dataItemBytes,0,dataItemBytes.Length, cancellationToken);
                await Response.Body.FlushAsync(cancellationToken);
            }

        }
    }
}
