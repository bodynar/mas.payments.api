using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MAS.Payments.ActionFilters;
using MAS.Payments.Commands;
using MAS.Payments.Infrastructure;
using MAS.Payments.MailMessages;
using MAS.Payments.Models;
using MAS.Payments.Notifications;

using Microsoft.AspNetCore.Mvc;

namespace MAS.Payments.Controllers
{
    [Authorize]
    [Route("api/user")]
    public class UserApiController : BaseApiController
    {
        public UserApiController(
            IResolver resolver
        ) : base(resolver)
        {
        }

        [HttpGet("[action]")]
        public IEnumerable<GetNotificationsResponse> GetNotifications()
        {
            return
                NotificationProcessor
                    .GetNotifications()
                    .Select(notification => new GetNotificationsResponse
                    {
                        Name = notification.Name,
                        Description = notification.Description,
                        Type = Enum.GetName(typeof(NotificationType), notification.Type)
                    });
        }

        [HttpPost("[action]")]
        public void TestMailMessage([FromBody]TestMailMessageRequest request)
        {
            MailProcessor.Send(new TestMailMessage(request.Recipient));
        }

        [HttpPost("[action]")]
        public void TestMailWithModelMessage([FromBody]TestMailMessageRequest request)
        {
            MailProcessor.Send(new TestMailMessageWithModel(request.Recipient, request.Counter, request.Name));
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public void Register([FromBody]RegistrationRequest request)
        {
            var command =
                new RegisterUserCommand(
                    request.Login, request.PasswordHash, request.Email,
                    request.FirstName, request.LastName);

            CommandProcessor.Execute(command);

            var encodedToken = HttpUtility.UrlEncode($"{command.Token}");

            MailProcessor.Send(
                new ConfirmRegistrationMailMessage(
                    $"https://{Request.Host.Host}:{Request.Host.Port}/confirmRegistration?token={encodedToken}",
                    request.Email, command.Token,
                    request.FirstName, request.LastName));
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public void ConfirmRegistration([FromBody]string token)
        {
            CommandProcessor.Execute(new ConfirmRegistrationCommand(token));
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public void ForgotPassword([FromBody]ForgotPasswordRequest request)
        {
            var command =
                new ResetPasswordCommand(request.Email, request.Login);

            CommandProcessor.Execute(command);

            var encodedToken = HttpUtility.UrlEncode($"{command.Token}");

            MailProcessor.Send(
                new ResetPasswordMailMessage(
                    $"https://{Request.Host.Host}:{Request.Host.Port}/resetPassword?token={encodedToken}",
                    request.Email, command.Token,
                    command.FirstName, command.LastName));
        }

        // [AllowAnonymous]
        // [HttpPost("[action]")]
        // public void UpdatePassword([FromBody]UpdatePassword request)
        // {

        // }
    }
}