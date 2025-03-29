namespace MAS.Payments.Infrastructure.Middleware
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using Newtonsoft.Json;
    using Serilog;

    public class ExceptionHandlerMiddleware(
        RequestDelegate next
    )
    {
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e).ConfigureAwait(false);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            Log.Error(exception, "{0}");

            var serializedException = JsonConvert.SerializeObject(new
            {
                Success = false,
                exception.Message,
                Stack = exception.StackTrace,
            });

            return context.Response.WriteAsync(serializedException);
        }
    }
}