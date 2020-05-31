using System;
using System.Collections.Generic;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Notifications
{
    internal abstract class BaseNotificator : INotificator
    {
        protected IResolver Resolver { get; }

         private Lazy<IQueryProcessor> _queryProcessor
            => new Lazy<IQueryProcessor>(() => Resolver.Resolve<IQueryProcessor>());

        protected IQueryProcessor QueryProcessor
            => _queryProcessor.Value;

        private Lazy<ICommandProcessor> _commandProcessor
            => new Lazy<ICommandProcessor>(() => Resolver.Resolve<ICommandProcessor>());

        protected ICommandProcessor CommandProcessor
            => _commandProcessor.Value;

        public BaseNotificator(
            IResolver resolver
        )
        {
            Resolver = resolver;
        }

        public abstract IEnumerable<Notification> GetNotifications();
    }
}