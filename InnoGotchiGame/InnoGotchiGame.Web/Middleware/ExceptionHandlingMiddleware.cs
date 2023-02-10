using InnoGotchiGame.Web.Models.ErrorModel;
using LoggerService;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace InnoGotchiGame.Web.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILoggerManager loggerManager)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex, context, loggerManager);
            }
        }

        private async Task HandleExceptionAsync(Exception exception, HttpContext context, ILoggerManager logger)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            logger.LogError($"Something went wrong: {exception}");
            await context.Response.WriteAsync(new ErrorDetails(context.Response.StatusCode, "Internal Server Error.").ToString());        
        }
    }
}
