using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebUI.Middleware.ExceptionMiddleWare
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleWare> _logger;

        public ExceptionMiddleWare(RequestDelegate next, ILogger<ExceptionMiddleWare> logger)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            _logger.LogError($"Something went wrong: ", exception.Message);
            var message = "Internal server error.";

            switch (exception)
            {
                case HttpRequestException:
                    context.Response.Redirect($"/reqerror");

                    break;
                default:
                    message = "Internal server error";
                    await context.Response.WriteAsync(new ErrorDetails
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = message
                    }.ToString());
                    break;
            }

            /* exception switch
         {
             DefaultGuidException => "A token cannot be default.",
             SelfParticipationException => "A player cannot participate in a game created by itself.",
             NotFoundException => "Spel was not found.",
             _ => "Internal server error."
         };*/
            ;

            /*await context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = message 
            }.ToString());*/
        }
    }
}
