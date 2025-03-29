namespace MAS.Payments.Infrastructure.Query
{
    using System;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure.Command;

    internal abstract class BaseQueryHandler<TQuery, TResult>(
        IResolver resolver
    ) : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        #region Private fields

        private Lazy<IQueryProcessor> queryProcessor
            => new(Resolver.Resolve<IQueryProcessor>);

        private Lazy<ICommandProcessor> commandProcessor
            => new(Resolver.Resolve<ICommandProcessor>);

        #endregion

        protected IResolver Resolver { get; } = resolver;

        protected IQueryProcessor QueryProcessor
            => queryProcessor.Value;

        protected ICommandProcessor CommandProcessor
            => commandProcessor.Value;

        public abstract TResult Handle(TQuery query);

        protected IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : Entity
        {
            return Resolver.Resolve<IRepository<TEntity>>();
        }
    }
}