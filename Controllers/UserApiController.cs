using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.MailMessaging;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Models;
using MAS.Payments.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace MAS.Payments.Controllers
{
    [Route("api/user")]
    public class UserApiController : BaseApiController
    {
        public IMailSender MailSender { get; }

        public IMailBuilder MailBuilder { get; }

        public UserApiController(
            ICommandProcessor commandProcessor,
            IQueryProcessor queryProcessor,
            INotificationProcessor notificationProcessor,
            IMailSender mailSender,
            IMailBuilder mailBuilder
        ) : base(commandProcessor, queryProcessor, notificationProcessor)
        {
            MailSender = mailSender;
            MailBuilder = mailBuilder;
        }

        [HttpGet("[action]")]
        public IEnumerable<GetNotificationsResponse> GetNotifications()
        {
            TestMailMessage();

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
        public Task TestMailMessage()
        {
            return MailSender.SendMailAsync(MailBuilder.FormTestMailMessage("bodynar@gmail.com"));
        }
    }
}