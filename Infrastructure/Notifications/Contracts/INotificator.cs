namespace MAS.Payments.Notifications
{
    using System.Collections.Generic;

    using MAS.Payments.DataBase;

    public interface INotificator
    {
        IEnumerable<UserNotification> GetNotifications();
    }
}