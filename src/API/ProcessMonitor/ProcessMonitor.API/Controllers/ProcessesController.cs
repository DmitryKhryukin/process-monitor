using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public async Task<IActionResult> GetProcessesAsync()
        {
            return StatusCode(StatusCodes.Status200OK, "processes");
        }
    }
}
