using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace Flare.Battleship.Api.Infrastructure.Exceptions
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            ErrorResponse response;
            switch (context.Exception)
            {
                case BadRequestException badRequest:
                    response = new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = badRequest.Message
                    };
                    break;

                default:
                    var exception = context.Exception;
                    _logger.LogError(exception, "GlobalExceptionFilter caught an exception.");
                    response = new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError,
                        Message = exception.Message,
                        Data = new { ExceptionType = exception.GetType().ToString() },
                        CallStack = exception.ToString().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    };
                    break;
            }
            context.Result = new ObjectResult(response)
            {
                StatusCode = response.StatusCode,
                ContentTypes = new MediaTypeCollection() { "application/problem+json" },
                DeclaredType = typeof(ErrorResponse)
            };
        }
    }
}

