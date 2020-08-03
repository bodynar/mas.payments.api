namespace MAS.Payments.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MAS.Payments.Commands;
    using MAS.Payments.DataBase;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.MailMessages;
    using MAS.Payments.Models;
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
        public async Task<IEnumerable<GetUserNotificationsQueryResult>> GetActiveUserNotifications()
        {
            var userNotifications = NotificationProcessor.GetNotifications();

            var notHiddenNotifications =
                QueryProcessor.Execute(new GetUserNotificationsQuery(GetUserNotificationsType.Visible));

            if (userNotifications.Any())
            {
                await Task.Run(
                    () => CommandProcessor.Execute(new AddUserNotificationCommand(userNotifications))
                ).ConfigureAwait(true);
            }

            return
                userNotifications
                .Select(notification => new GetUserNotificationsQueryResult
                {
                    Key = notification.Key,
                    Text = notification.Text,
                    Title = notification.Title,
                    CreatedAt = notification.CreatedAt,
                    Type = Enum.GetName(typeof(UserNotificationType), notification.Type)
                })
                .Union(notHiddenNotifications)
                .OrderBy(x => x.CreatedAt);
        }

        [HttpGet("[action]")]
        public IEnumerable<GetUserNotificationsQueryResult> GetUserNotifications()
        {
            return QueryProcessor.Execute(new GetUserNotificationsQuery(GetUserNotificationsType.All));
        }

        [HttpPost("[action]")]
        public void HideNotifications([FromBody] IEnumerable<string> userNotificationKeys)
        {
            CommandProcessor.Execute(new HideUserNotificationCommand(userNotificationKeys));
        }

        [HttpPost("[action]")]
        public void TestMailMessage([FromBody] TestMailMessageRequest request)
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
        public void UpdateUserSettings([FromBody] IEnumerable<UpdateUserSettingsRequest> settings)
        {
            CommandProcessor.Execute(new UpdateUserSettingsCommand(settings));
        }

        [HttpGet("[action]")]
        public IEnumerable<GetMailMessageLogsQueryResult> GetMailMessageLogs()
        {
            return QueryProcessor.Execute(new GetMailMessageLogsQuery());
        }
    }
}