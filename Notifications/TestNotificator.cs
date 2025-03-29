namespace MAS.Payments.Notifications
{
    using System.Collections.Generic;

    using MAS.Payments.DataBase;
    using MAS.Payments.Infrastructure;

    internal class TestNotificator(
        IResolver resolver
    ) : BaseNotificator(resolver)
    {
        public override async IAsyncEnumerable<UserNotification> GetNotificationsAsync()
        {
            var wasNotificationFormed = await CheckWasNotificationFormedAsync("testNotification");

            if (!wasNotificationFormed)
            {
                yield return new UserNotification
                {
                    Title = " Test ",
                    Text = " lorem ipsum ",
                    Key = "testNotification",
                    Type = (short)UserNotificationType.Info
                };
            }
        }
    }
}