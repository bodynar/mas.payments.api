namespace MAS.Payments.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Query;

    internal abstract class BaseNotificator(
        IResolver resolver
    ) : INotificator
    {

        private Lazy<IQueryProcessor> queryProcessor
            => new(resolver.Resolve<IQueryProcessor>);

        private Lazy<ICommandProcessor> commandProcessor
            => new(resolver.Resolve<ICommandProcessor>);

        private Lazy<IRepository<UserNotification>> userNotificationRepository
            => new(resolver.Resolve<IRepository<UserNotification>>);

        protected IQueryProcessor QueryProcessor
            => queryProcessor.Value;

        protected ICommandProcessor CommandProcessor
            => commandProcessor.Value;

        protected IRepository<UserNotification> Repository
            => userNotificationRepository.Value;

        public abstract IAsyncEnumerable<UserNotification> GetNotificationsAsync();

        protected async Task<bool> CheckWasNotificationFormedAsync(string notificationKey)
        {
            return await Repository.Any(new UserNotificationSpec.IsKeyIn(notificationKey));
        }
    }
}