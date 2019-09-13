using System;
using System.Collections.Generic;
using System.Linq;
using MAS.Payments.Infrastructure;
using MAS.Payments.MailMessages;
using MAS.Payments.Models;
using MAS.Payments.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace MAS.Payments.Controllers
{
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
    }
}