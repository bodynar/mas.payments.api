using System;
using System.Collections.Generic;
using System.Linq;
using MAS.Payments.Infrastructure;

namespace MAS.Payments.Notifications
{
    public class NotificationProcessor : INotificationProcessor
    {
        private IResolver Resolver { get; }

        private static Lazy<IEnumerable<Type>> NotificatorsTypesMap
            => new Lazy<IEnumerable<Type>>(() => FillNotificatorsTypesMap());

        public NotificationProcessor(
            IResolver resolver
        )
        {
            Resolver = resolver;
        }

        public IEnumerable<Notification> GetNotifications()
        {
            var notifications = new List<Notification>();

            foreach (var notificatorType in NotificatorsTypesMap.Value)
            {
                dynamic notificator = Resolver.GetInstance(notificatorType);

                notifications.AddRange(notificator.GetNotifications());
            }

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