namespace MAS.Payments.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using MAS.Payments.Commands;
    using MAS.Payments.DataBase;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.MailMessages;
    using MAS.Payments.Models;
    using MAS.Payments.Queries;
    using MAS.Payments.Utilities;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    [Route("api/user")]
    public class UserApiController : BaseApiController
    {
        private IConfiguration Configuration { get; }

        public UserApiController(
            IResolver resolver,
            IConfiguration configuration
        ) : base(resolver)
        {
            Configuration = configuration;
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
        public IEnumerable<long> HideNotifications([FromBody] IEnumerable<long> userNotificationIds)
        {
            if (userNotificationIds == null)
            {
                throw new ArgumentNullException(nameof(userNotificationIds));
            }

            var command = new HideUserNotificationCommand(userNotificationIds.Where(x => x != default));

            CommandProcessor.Execute(command);

            return command.NotProcessedIds;
        }

        [HttpPost("[action]")]
        public void TestMailMessage([FromBody] TestMailMessageRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

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
            if (settings == null || settings.Contains(null))
            {
                throw new ArgumentNullException(nameof(settings));
            }

            CommandProcessor.Execute(new UpdateUserSettingsCommand(settings));
        }

        [HttpGet("[action]")]
        public IEnumerable<GetMailMessageLogsQueryResult> GetMailMessageLogs()
        {
            return QueryProcessor.Execute(new GetMailMessageLogsQuery());
        }

        [HttpGet("[action]")]
        public GetAppInfoResponse GetAppInfo()
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var databaseName = Regex.Match(connectionString, @"\;Database=([^\;]*)\;");

            var response = new GetAppInfoResponse(databaseName.Groups[1]?.Value, GetType().Assembly.GetName().Version.ToString());

            return response;
        }
    }
}