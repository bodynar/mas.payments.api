using System;
using System.Linq;
using MAS.Payments.DataBase;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using SimpleInjector;

namespace MAS.Payments.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AuthorizeAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var allowAnonymous = context.Filters.Any(x => x is AllowAnonymousAttribute);

            if (!allowAnonymous)
            {
                bool isAuthorized = IsAuthorized(context.HttpContext);

                if (!isAuthorized)
                {
                    var methodName = context.ActionDescriptor.RouteValues["action"];
                    var controllerName = context.ActionDescriptor.RouteValues["controller"];
                    
                    context.Result = new Unauthorized(controllerName, methodName);
                }
            }
        }

        private bool IsAuthorized(HttpContext httpContext)
        {
            var token = httpContext.Request.Headers["auth-token"].FirstOrDefault();

            if (!string.IsNullOrEmpty(token))
            {
                var resolver =
                    httpContext.RequestServices.GetService(typeof(Container));

                if (resolver != null)
                {
                    var queryProcessor =
                        (resolver as Container).GetInstance<IQueryProcessor>();

                    if (queryProcessor != null)
                    {
                        var isTokenValid =
                            (queryProcessor as IQueryProcessor).Execute(
                                new IsTokenValidQuery(token, UserTokenTypeEnum.Auth));

                        return isTokenValid;
                    }
                }


            }

            return false;
        }
    }
}