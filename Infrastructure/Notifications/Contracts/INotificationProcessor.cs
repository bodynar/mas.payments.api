using System.Collections.Generic;

namespace MAS.Payments.Notifications
{
    public interface INotificationProcessor
    {
        IEnumerable<Notification> GetNotifications();
    }
}