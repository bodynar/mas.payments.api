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
            => new Lazy<IEnumerable<Type>>(() => FillNotificatorsTypesMap<IPublicNotificator>());

        private static Lazy<IEnumerable<Type>> UserNotificatorsTypesMap
            => new Lazy<IEnumerable<Type>>(() => FillNotificatorsTypesMap<IUserNotificator>());


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

        public IEnumerable<Notification> GetUserNotifications(long userId)
        {
            var notifications = new List<Notification>();

            foreach (var notificatorType in UserNotificatorsTypesMap.Value)
            {
                dynamic notificator = Resolver.GetInstance(notificatorType);

                notifications.AddRange(notificator.GetUserNotifications(userId));
            }

            return notifications;
        }

        private static IEnumerable<Type> FillNotificatorsTypesMap<TNotification>()
            where TNotification: INotificator
        {
            var notificatorInterfaceType = typeof(TNotification);

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