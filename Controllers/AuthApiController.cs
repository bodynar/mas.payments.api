using System;

using MAS.Payments.ActionFilters;
using MAS.Payments.Commands;
using MAS.Payments.DataBase;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Cache;
using MAS.Payments.Models;
using MAS.Payments.Queries;

using Microsoft.AspNetCore.Mvc;

namespace MAS.Payments.Controllers
{
    [Route("api/auth")]
    public class AuthApiController : BaseApiController
    {
        public AuthApiController(
            IResolver resolver
        ) : base(resolver) { }

        [HttpPost("[action]")]
        public string Authenticate([FromBody]AuthenticateRequest request)
        {
            var command =
                new AuthenticateCommand(
                    request.Login, request.PasswordHash, request.RememberMe);

            CommandProcessor.Execute(command);

            AuthTokensCache.Save(command.Token);

            return command.Token.Token;
        }

        [Authorize]
        [HttpPost("[action]")]
        public void Logout([FromBody]string token)
        {
            CommandProcessor.Execute(new LogoutCommand(token));

            AuthTokensCache.Remove(token);
        }

        [HttpGet("[action]")]
        public bool IsTokenValid(string token)
        {
            var cachedToken = AuthTokensCache.Get(token);

            if (cachedToken != null)
            {
                var minutesSinceLastCheck = (DateTime.UtcNow - cachedToken.LastChecked).TotalMinutes;

                if (minutesSinceLastCheck > 5)
                {
                    return IsTokenValid(token, cachedToken);
                }

                return true;
            }

            return IsTokenValid(token);
        }

        private bool IsTokenValid(string token, CachedAuthToken cachedAuthToken = null)
        {
            var query = new IsTokenValidQuery(token, UserTokenTypeEnum.Auth);

            var isTokenValid = QueryProcessor.Execute(query);

            // todo: test
            UpdateCachedToken(cachedAuthToken, token, query.UserToken, isTokenValid);

            return isTokenValid;
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
                        UserLogin = userToken.User.Login
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