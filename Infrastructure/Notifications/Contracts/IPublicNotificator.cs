using System.Collections.Generic;

namespace MAS.Payments.Notifications
{
    public interface IPublicNotificator: INotificator
    {
        IEnumerable<Notification> GetNotifications();
    }
}