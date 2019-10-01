using System;
using System.Collections.Generic;
using System.Linq;
using MAS.Payments.Commands;
using MAS.Payments.Infrastructure;
using MAS.Payments.MailMessages;
using MAS.Payments.Models;
using MAS.Payments.Notifications;
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

        public void LogOff()
        {
            throw new NotImplementedException();
        }
    }
}