using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ProcessMonitor.API.Filters
{
    public class RequestCancelledExceptionFilter: ExceptionFilterAttribute
    {
        private readonly ILogger<RequestCancelledExceptionFilter> _logger;

        public RequestCancelledExceptionFilter(ILogger<RequestCancelledExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            if(context.Exception is TaskCanceledException || context.Exception is OperationCanceledException)
            {
                _logger.LogInformation("Request was cancelled");
                context.ExceptionHandled = true;
            }
        }
    }
}
