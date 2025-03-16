namespace MAS.Payments.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.Infrastructure;

    public class NotificationProcessor(
        IResolver resolver
    ) : INotificationProcessor
    {
        private static Lazy<IEnumerable<Type>> NotificatorsTypesMap
            => new(FillNotificatorsTypesMap);

        public IEnumerable<UserNotification> GetNotifications()
        {
            var notifications = new List<UserNotification>();

            foreach (var notificatorType in NotificatorsTypesMap.Value)
            {
                dynamic notificator = resolver.GetInstance(notificatorType);

                notifications.AddRange(notificator.GetNotifications());
            }

            notifications.ForEach(x => x.CreatedAt = DateTime.Now);

            return notifications;
        }

        private static IEnumerable<Type> FillNotificatorsTypesMap()
        {
            var notificatorInterfaceType = typeof(INotificator);

            return
                 typeof(NotificationProcessor)
                     .Assembly
                     .GetTypes()
                     .Where(type =>
                         type.IsClass
                         && !type.IsAbstract
                         && type.GetInterfaces().Contains(notificatorInterfaceType));
        }
    }
}