using System.Collections.Generic;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Notifications
{
    internal abstract class BaseNotificator : INotificator
    {
        protected IResolver Resolver { get; }

        private IQueryProcessor _queryProcessor;

        protected IQueryProcessor QueryProcessor
            => _queryProcessor ?? Resolver.Resolve<IQueryProcessor>();


        private ICommandProcessor _commandProcessor;

        protected ICommandProcessor CommandProcessor
            => _commandProcessor ?? Resolver.Resolve<ICommandProcessor>();

        public BaseNotificator(
            IResolver resolver
        )
        {
            Resolver = resolver;
        }

        public abstract IEnumerable<Notification> GetNotifications();
    }
}