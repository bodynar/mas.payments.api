using System.Collections.Generic;
using MAS.Payments.DataBase;

namespace MAS.Payments.Notifications
{
    public interface INotificationProcessor
    {
        IEnumerable<UserNotification> GetNotifications();
    }
}