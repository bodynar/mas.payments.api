using System;
using MAS.Payments.Commands;
using MAS.Payments.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace MAS.Payments.Controllers
{
    [Route("api/auth")]
    public class AuthApiController : BaseApiController
    {
        public AuthApiController(
            IResolver resolver
        ) : base(resolver)
        {
        }

        public string Authenticate()
        {
            throw new NotImplementedException();
        }

        [HttpPost("[action]")]
        public void LogOff([FromBody]string token)
        {
            CommandProcessor.Execute(new LogOffCommand(token));
        }
    }
}