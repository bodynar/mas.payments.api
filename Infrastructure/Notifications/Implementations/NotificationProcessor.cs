namespace MAS.Payments.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.Infrastructure;

    public class NotificationProcessor(
        IResolver resolver
    ) : INotificationProcessor
    {
        private static Lazy<IEnumerable<Type>> NotificatorsTypesMap
            => new(FillNotificatorsTypesMap);

        public async Task<IEnumerable<UserNotification>> GetNotificationsAsync()
        {
            var notifications = new List<UserNotification>();

            foreach (var notificatorType in NotificatorsTypesMap.Value)
            {
                var notificator = (INotificator)resolver.GetInstance(notificatorType);

                await foreach (var notification in notificator.GetNotificationsAsync())
                {
                    notifications.Add(notification);
                }
            }

            notifications.ForEach(x => x.CreatedAt = DateTime.UtcNow);

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
