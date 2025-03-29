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
    using MAS.Payments.Models;
    using MAS.Payments.Queries;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    [Route("api/user")]
    public partial class UserApiController(
        IResolver resolver,
        IConfiguration configuration
    ) : BaseApiController(resolver)
    {
        [HttpGet("[action]")]
        public async Task<IEnumerable<GetUserNotificationsQueryResult>> GetActiveUserNotifications()
        {
            var userNotifications = NotificationProcessor.GetNotifications();

            var notHiddenNotifications =
                await QueryProcessor.Execute(new GetUserNotificationsQuery(GetUserNotificationsType.Visible));

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
        public async Task<IEnumerable<GetUserNotificationsQueryResult>> GetUserNotifications()
        {
            return await QueryProcessor.Execute(new GetUserNotificationsQuery(GetUserNotificationsType.All));
        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<long>> HideNotifications([FromBody] IEnumerable<long> userNotificationIds)
        {
            ArgumentNullException.ThrowIfNull(userNotificationIds);

            var command = new HideUserNotificationCommand(userNotificationIds.Where(x => x != default));

            await CommandProcessor.Execute(command);

            return command.NotProcessedIds;
        }

        [HttpGet("[action]")]
        public async Task<IReadOnlyCollection<GetUserSettingsQueryResult>> GetSettings()
        {
            return await QueryProcessor.Execute(new GetUserSettingsQuery());
        }

        [HttpPost("[action]")]
        public async Task UpdateUserSettings([FromBody] IEnumerable<UpdateUserSettingsRequest> settings)
        {
            if (settings == null || settings.Contains(null))
            {
                throw new ArgumentNullException(nameof(settings));
            }

            await CommandProcessor.Execute(new UpdateUserSettingsCommand(settings));
        }

        [HttpGet("[action]")]
        public GetAppInfoResponse GetAppInfo()
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var databaseName = DatabaseNameRegex().Match(connectionString);

            var response = new GetAppInfoResponse(databaseName.Groups[1]?.Value, GetType().Assembly.GetName().Version.ToString());

            return response;
        }

        [GeneratedRegex(@"Database=([^\;]*)\;")]
        private static partial Regex DatabaseNameRegex();
    }
}