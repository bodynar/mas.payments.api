using System.Collections.Generic;

namespace MAS.Payments.Notifications
{
    public interface IUserNotificator: INotificator
    {
        IEnumerable<Notification> GetNotifications(long userId);
    }
}