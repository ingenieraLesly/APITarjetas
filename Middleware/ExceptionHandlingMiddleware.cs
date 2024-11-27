using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PrimoAPITarjetas.Models;


namespace PrimoAPITarjetas.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorDetails = new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error from the custom middleware."
            };

            // Convert the errorDetails object to JSON using Newtonsoft.Json
            var json = JsonConvert.SerializeObject(errorDetails);

            return context.Response.WriteAsync(json);
        }
    }
}
