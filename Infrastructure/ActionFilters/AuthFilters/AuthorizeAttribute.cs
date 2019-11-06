using System;
using System.Collections.Generic;
using System.Linq;

using MAS.Payments.DataBase;
using MAS.Payments.Infrastructure.Cache;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Models;
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
                // solve about storing user tokens
                var cachedTokens = CacheService.Get<IEnumerable<CachedAuthToken>>("authTokens");

                var cachedToken = cachedTokens.FirstOrDefault(x => x.Token == token);
                
                if (cachedToken != null)
                {
                    var minutesSinceLastCheck = Math.Round((DateTime.Now - cachedToken.LastChecked).TotalMinutes);

                    if (minutesSinceLastCheck > 5)
                    {
                        return IsTokenValid(token, httpContext);
                    }

                    return true;
                }

                return IsTokenValid(token, httpContext);
            }

            return false;
        }

        private bool IsTokenValid(string token, HttpContext httpContext)
        {
            // get token
            // check is valid
            // save valid info into cache

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

            throw new Exception("QueryProcessor cannot be constructed");
        }
    }
}