namespace MAS.Payments.Infrastructure.Command
{
    using System;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure.MailMessaging;
    using MAS.Payments.Infrastructure.Query;

    public abstract class BaseCommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
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

        protected Type CommandType { get; }

        public BaseCommandHandler(
            IResolver resolver
        )
        {
            Resolver = resolver;

            CommandType = typeof(TCommand);
        }

        public abstract void Handle(TCommand command);

        protected IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : Entity
        {
            return Resolver.Resolve<IRepository<TEntity>>();
        }
    }
}