namespace MAS.Payments.Commands
{
    using System.Collections.Generic;

    using MAS.Payments.DataBase;
    using MAS.Payments.Infrastructure.Command;

    public class AddUserNotificationCommand: ICommand
    {
        public IEnumerable<UserNotification> UserNotifications { get; }

        public AddUserNotificationCommand(IEnumerable<UserNotification> userNotifications)
        {
            UserNotifications = new List<UserNotification>(userNotifications);
        }

        public AddUserNotificationCommand(UserNotification userNotification)
        {
            UserNotifications = new List<UserNotification>() { userNotification };
        }
    }
}