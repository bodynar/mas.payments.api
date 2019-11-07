using System;
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
                var cachedToken = AuthTokensCache.Get(token);

                if (cachedToken != null)
                {
                    var minutesSinceLastCheck = (DateTime.UtcNow - cachedToken.LastChecked).TotalMinutes;

                    if (minutesSinceLastCheck > 5)
                    {
                        return IsTokenValid(token, httpContext, cachedToken);
                    }

                    return true;
                }

                return IsTokenValid(token, httpContext);
            }

            return false;
        }

        private bool IsTokenValid(string token, HttpContext httpContext, CachedAuthToken cachedAuthToken = null)
        {
            var resolver =
                httpContext.RequestServices.GetService(typeof(Container));

            if (resolver != null)
            {
                var queryProcessor =
                    (resolver as Container).GetInstance<IQueryProcessor>();

                if (queryProcessor != null)
                {
                    var query = new IsTokenValidQuery(token, UserTokenTypeEnum.Auth);

                    var isTokenValid =
                        (queryProcessor as IQueryProcessor).Execute(query);

                    // todo: test
                    UpdateCachedToken(cachedAuthToken, token, query.UserToken, isTokenValid);

                    return isTokenValid;
                }
            }

            throw new Exception("QueryProcessor cannot be constructed");
        }

        private void UpdateCachedToken(CachedAuthToken cachedAuthToken, string token, UserToken userToken, bool isTokenValid)
        {
            if (isTokenValid)
            {
                if (cachedAuthToken == null)
                {
                    cachedAuthToken = new CachedAuthToken
                    {
                        Token = token,
                        ActiveTo = userToken.ActiveTo.Value,
                        CreatedAt = userToken.CreatedAt,
                        LastChecked = DateTime.UtcNow,
                        UserId = userToken.UserId,
                        UserName = userToken.User.Login
                    };
                }
                else
                {
                    cachedAuthToken.LastChecked = DateTime.UtcNow;
                }

                AuthTokensCache.SaveOrUpdate(cachedAuthToken);
            }
            else
            {
                if (cachedAuthToken != null)
                {
                    AuthTokensCache.Remove(token);
                }
            }
        }
    }
}