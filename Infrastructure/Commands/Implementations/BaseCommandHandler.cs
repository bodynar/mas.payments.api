namespace MAS.Payments.Infrastructure.Command
{
    using System;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure.Query;

    public abstract class BaseCommandHandler<TCommand>(
        IResolver resolver
    ) : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        #region Private fields

        private IQueryProcessor queryProcessor;

        private ICommandProcessor commandProcessor;

        #endregion

        protected IQueryProcessor QueryProcessor
            => queryProcessor ??= resolver.Resolve<IQueryProcessor>();

        protected ICommandProcessor CommandProcessor
            => commandProcessor ??= resolver.Resolve<ICommandProcessor>();

        protected Type CommandType { get; } = typeof(TCommand);

        public abstract Task HandleAsync(TCommand command);

        protected IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : Entity
        {
            return resolver.Resolve<IRepository<TEntity>>();
        }
    }
}