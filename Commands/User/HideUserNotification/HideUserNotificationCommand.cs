namespace MAS.Payments.Commands
{
    using System.Collections.Generic;

    using MAS.Payments.Infrastructure.Command;

    public class HideUserNotificationCommand : ICommand
    {
        public IEnumerable<long> NotificationIds { get; }

        public HideUserNotificationCommand(IEnumerable<long> notificationIds)
        {
            NotificationIds = new List<long>(notificationIds);
        }

        public HideUserNotificationCommand(long notificationId)
        {
            NotificationIds = new List<long>() { notificationId };
        }
    }
}