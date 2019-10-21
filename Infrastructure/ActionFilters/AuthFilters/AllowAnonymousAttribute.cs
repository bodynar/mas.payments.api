using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MAS.Payments.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowAnonymousAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
        }
    }
}