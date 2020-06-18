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
        public async Task<IEnumerable<GetNotificationsResponse>> GetNotifications()
        {
            var userNotifications = NotificationProcessor.GetNotifications();

            if (userNotifications.Any())
            {
                await Task.Run(
                    () => CommandProcessor.Execute(new AddUserNotificationCommand(userNotifications))
                ).ConfigureAwait(true);
            }

            var notHiddenNotifications =
                QueryProcessor.Execute(new GetUserNotificationsQuery(GetUserNotificationsType.Hidden));

            return
                userNotifications
                .Union(notHiddenNotifications)
                .Select(notification => new GetNotificationsResponse
                {
                    Name = notification.Title,
                    Description = notification.Text,
                    Type = Enum.GetName(typeof(UserNotificationType), notification.Type)
                });
        }

        [HttpPost("[action]")]
        public void HideNotifications([FromBody]IEnumerable<long> userNotificationIds)
        {
            CommandProcessor.Execute(new HideUserNotificationCommand(userNotificationIds));
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