using System;
using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Infrastructure.Command
{
    internal abstract class BaseCommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
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

        public BaseCommandHandler(
            IResolver resolver
        )
        {
            Resolver = resolver;
        }

        public abstract void Handle(TCommand command);

        protected IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : Entity
        {
            return Resolver.Resolve<IRepository<TEntity>>();
        }

        protected IUnitOfWork CreateUnitOfWorkScope()
        {
            return Resolver.Resolve<IUnitOfWork>();
        }
    }
}