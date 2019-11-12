using System.Collections.Generic;

using MAS.Payments.Infrastructure;

namespace MAS.Payments.Notifications
{
    internal abstract class BasePublicNotificator : BaseNotificator, IPublicNotificator
    {
        public BasePublicNotificator(IResolver resolver)
            : base(resolver)
        { }

        public abstract IEnumerable<Notification> GetNotifications();
    }
}