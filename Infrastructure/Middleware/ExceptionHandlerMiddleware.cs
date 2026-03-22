namespace MAS.Payments.Infrastructure.Middleware
{
    using System;
    using System.IO;
    using System.Linq;
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
            httpContext.Request.EnableBuffering();

            try
            {
                await next(httpContext).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e).ConfigureAwait(false);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var endpoint = context.GetEndpoint();
            var actionName = endpoint?.Metadata?.GetMetadata<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>();

            var controllerFile = actionName != null
                ? $"{actionName.ControllerTypeInfo.Name}.cs"
                : "Unknown";

            var methodName = actionName?.ActionName ?? "Unknown";

            var queryParams = context.Request.QueryString.HasValue
                ? context.Request.QueryString.Value
                : null;

            string body = null;
            if (context.Request.Body != null && context.Request.ContentLength > 0)
            {
                context.Request.Body.Position = 0;
                using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                body = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            var requestParams = new { query = queryParams, body };
            var requestParamsJson = JsonConvert.SerializeObject(requestParams, Formatting.None);

            Log.Error(
                "File: {ControllerFile}, Method: {MethodName}, Params: {RequestParams}\n{StackTrace}\n{ErrorMessage}",
                controllerFile, methodName, requestParamsJson, exception.StackTrace, exception.Message);

            var serializedException = JsonConvert.SerializeObject(new
            {
                Success = false,
                Message = "An internal server error has occurred.",
            });

            await context.Response.WriteAsync(serializedException).ConfigureAwait(false);
        }
    }
}
