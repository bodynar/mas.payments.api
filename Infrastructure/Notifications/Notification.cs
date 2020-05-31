namespace MAS.Payments.Notifications
{
    public enum NotificationType
    {
        Info,
        Warning,
    }

    public class Notification
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public NotificationType Type { get; set; }
    }
}