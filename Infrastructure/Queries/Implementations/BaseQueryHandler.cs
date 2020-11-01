namespace MAS.Payments.Infrastructure.Query
{
    using System;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.MailMessaging;

    internal abstract class BaseQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        #region Private fields

        private Lazy<IQueryProcessor> _queryProcessor
            => new Lazy<IQueryProcessor>(Resolver.Resolve<IQueryProcessor>);

        private Lazy<ICommandProcessor> _commandProcessor
            => new Lazy<ICommandProcessor>(Resolver.Resolve<ICommandProcessor>);

        private Lazy<IMailProcessor> _mailProcessor
            => new Lazy<IMailProcessor>(Resolver.Resolve<IMailProcessor>);

        #endregion

        protected IResolver Resolver { get; }

        protected IQueryProcessor QueryProcessor
            => _queryProcessor.Value;

        protected ICommandProcessor CommandProcessor
            => _commandProcessor.Value;

        protected IMailProcessor MailProcessor
            => _mailProcessor.Value;

        public BaseQueryHandler(
            IResolver resolver
        )
        {
            Resolver = resolver;
        }

        public abstract TResult Handle(TQuery query);

        protected IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : Entity
        {
            return Resolver.Resolve<IRepository<TEntity>>();
        }
    }
}