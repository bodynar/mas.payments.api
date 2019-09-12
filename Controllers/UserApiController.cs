using System;
using System.Collections.Generic;
using System.Linq;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.MailMessaging;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.MailMessages;
using MAS.Payments.Models;
using MAS.Payments.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace MAS.Payments.Controllers
{
    [Route("api/user")]
    public class UserApiController : BaseApiController
    {
        public IMailProcessor MailProcessor { get; }

        public UserApiController(
            ICommandProcessor commandProcessor,
            IQueryProcessor queryProcessor,
            INotificationProcessor notificationProcessor,
            IMailProcessor mailProcessor
        ) : base(commandProcessor, queryProcessor, notificationProcessor)
        {
            MailProcessor = mailProcessor;
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
        public void TestMailMessage(string reciepent)
        {
            MailProcessor.Send(new TestMailMessage(reciepent));
        }

        [HttpPost("[action]")]
        public void TestMailWithModelMessage(string reciepent, int counter, string name)
        {
            MailProcessor.Send(new TestMailMessageWithModel(reciepent, counter, name));
        }
    }
}