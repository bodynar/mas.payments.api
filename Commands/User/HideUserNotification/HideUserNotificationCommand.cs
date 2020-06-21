namespace MAS.Payments.Commands
{
    using System.Collections.Generic;

    using MAS.Payments.Infrastructure.Command;

    public class HideUserNotificationCommand : ICommand
    {
        public IEnumerable<string> NotificationKeys { get; }

        public HideUserNotificationCommand(IEnumerable<string> notificationKeys)
        {
            NotificationKeys = new List<string>(notificationKeys);
        }

        public HideUserNotificationCommand(string notificationKey)
        {
            NotificationKeys = new List<string>() { notificationKey };
        }
    }
}