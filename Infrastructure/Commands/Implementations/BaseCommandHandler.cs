using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Infrastructure.Command
{
    internal class BaseCommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        protected IResolver Resolver { get; }

        private IQueryProcessor _queryProcessor;

        protected IQueryProcessor QueryProcessor
            => _queryProcessor ?? Resolver.Resolve<IQueryProcessor>();


        private ICommandProcessor _commandProcessor;

        protected ICommandProcessor CommandProcessor
            => _commandProcessor ?? Resolver.Resolve<ICommandProcessor>();

        public BaseCommandHandler(
            IResolver resolver
        )
        {
            Resolver = resolver;
        }

        public virtual void Handle(TCommand command)
        {

        }

        protected IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class
        {
            return Resolver.Resolve<IRepository<TEntity>>();
        }

        protected IUnitOfWork CreateUnitOfWorkScope()
        {
            return Resolver.Resolve<IUnitOfWork>();
        }
    }
}