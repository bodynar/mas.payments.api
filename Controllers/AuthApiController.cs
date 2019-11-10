using MAS.Payments.ActionFilters;
using MAS.Payments.Commands;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Cache;
using MAS.Payments.Models;

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
    }
}