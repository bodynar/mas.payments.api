namespace MAS.Payments.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MAS.Payments.Commands;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.MailMessages;
    using MAS.Payments.Models;
    using MAS.Payments.Notifications;
    using MAS.Payments.Queries;
    using MAS.Payments.Utilities;

    using Microsoft.AspNetCore.Mvc;

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
            var isValidEmail = Validate.Email(request.Recipient);

            if (!isValidEmail)
            {
                throw new ArgumentException($"Passed \"{request.Recipient}\" isn't recognized as email.");
            }

            MailProcessor.Send(new TestMailMessage(request.Recipient));
        }

        [HttpGet("[action]")]
        public IReadOnlyCollection<GetUserSettingsQueryResult> GetSettings()
        {
            return QueryProcessor.Execute(new GetUserSettingsQuery());
        }

        [HttpPost("[action]")]
        public void UpdateUserSettings([FromBody]IEnumerable<UpdateUserSettingsRequest> settings)
        {
            CommandProcessor.Execute(new UpdateUserSettingsCommand(settings));
        }
    }
}