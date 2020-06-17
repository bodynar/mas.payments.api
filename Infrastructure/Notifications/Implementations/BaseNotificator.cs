namespace MAS.Payments.Notifications
{
    using System;
    using System.Collections.Generic;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Query;

    internal abstract class BaseNotificator : INotificator
    {
        protected IResolver Resolver { get; }

        private Lazy<IQueryProcessor> _queryProcessor
            => new Lazy<IQueryProcessor>(() => Resolver.Resolve<IQueryProcessor>());

        private Lazy<ICommandProcessor> _commandProcessor
            => new Lazy<ICommandProcessor>(() => Resolver.Resolve<ICommandProcessor>());

        private Lazy<IRepository<UserNotification>> _userNotificationRepository
            => new Lazy<IRepository<UserNotification>>(() => Resolver.Resolve<IRepository<UserNotification>>());

        protected IQueryProcessor QueryProcessor
            => _queryProcessor.Value;

        protected ICommandProcessor CommandProcessor
            => _commandProcessor.Value;

        protected IRepository<UserNotification> Repository
            => _userNotificationRepository.Value;

        public BaseNotificator(
            IResolver resolver
        )
        {
            Resolver = resolver;
        }

        public abstract IEnumerable<UserNotification> GetNotifications();

        protected bool CheckWasNotificationFormed(string notificationKey)
        {
            return Repository.Any(new UserNotificationSpec.IsKeyIn(notificationKey));
        }
    }
}