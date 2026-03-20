namespace MAS.Payments.Notifications
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;

    public interface INotificationProcessor
    {
        Task<IEnumerable<UserNotification>> GetNotificationsAsync();
    }
}
