using System.Collections.Generic;

using MAS.Payments.Infrastructure;

namespace MAS.Payments.Notifications
{
    internal class TestNotificator : BasePublicNotificator
    {
        public TestNotificator(
            IResolver resolver
        ) : base(resolver)
        {
        }

        public override IEnumerable<Notification> GetNotifications()
        {
            yield return new Notification
            {
                Name = " Test ",
                Description = " lorem ipsum ",
                Type = NotificationType.Info
            };
        }
    }
}