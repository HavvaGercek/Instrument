using Instrument.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Instrument.API.Filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, context.Exception.Message);
            int status = (int)HttpStatusCode.InternalServerError;
            context.HttpContext.Response.StatusCode = status;
            context.Result = new JsonResult(new ExceptionResponse
            {
                Message = "An error occured.",
                Status = status
            });
        }
    }
}
