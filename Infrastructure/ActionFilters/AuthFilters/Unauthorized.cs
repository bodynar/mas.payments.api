using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MAS.Payments.ActionFilters
{
    public class Unauthorized : IActionResult
    {
        public string ActionName { get; }

        public string ControllerName { get; }

        public Unauthorized(string controllerName, string actionName)
        {
            ControllerName = controllerName.ToLower();
            ActionName = actionName.ToLower();
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var errorObject = new {
                HasError = true,
                ErrorMessage = $"Access denied on \"{ControllerName}\\{ActionName}\" action."
            };

            var objectResult = new JsonResult(errorObject)
            {
                StatusCode = 401,
            };

            context.HttpContext.Request.Headers.Remove("auth-token");

            await objectResult.ExecuteResultAsync(context);
        }
    }
}