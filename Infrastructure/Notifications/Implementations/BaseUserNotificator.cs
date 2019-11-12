using System.Collections.Generic;

using MAS.Payments.Infrastructure;

namespace MAS.Payments.Notifications
{
    internal abstract class BaseUserNotificator : BaseNotificator, IUserNotificator
    {
        public BaseUserNotificator(IResolver resolver)
            : base(resolver)
        { }

        public abstract IEnumerable<Notification> GetNotifications(long userId);
    }
}