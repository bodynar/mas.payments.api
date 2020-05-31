using System.Collections.Generic;

namespace MAS.Payments.Notifications
{
    public interface INotificator
    {
        IEnumerable<Notification> GetNotifications();
    }
}