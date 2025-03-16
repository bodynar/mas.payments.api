namespace MAS.Payments.Infrastructure.Command
{
    using System;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure.Query;

    public abstract class BaseCommandHandler<TCommand>(
        IResolver resolver
    ) : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        #region Private fields

        private Lazy<IQueryProcessor> queryProcessor
            => new(resolver.Resolve<IQueryProcessor>);

        private Lazy<ICommandProcessor> commandProcessor
            => new(resolver.Resolve<ICommandProcessor>);

        #endregion

        protected IQueryProcessor QueryProcessor
            => queryProcessor.Value;

        protected ICommandProcessor CommandProcessor
            => commandProcessor.Value;

        protected Type CommandType { get; } = typeof(TCommand);

        public abstract void Handle(TCommand command);

        protected IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : Entity
        {
            return resolver.Resolve<IRepository<TEntity>>();
        }
    }
}