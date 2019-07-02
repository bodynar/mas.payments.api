using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Infrastructure.Query
{
    internal class BaseQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        protected IResolver Resolver { get; }

        private IQueryProcessor _queryProcessor;

        protected IQueryProcessor QueryProcessor
            => _queryProcessor ?? Resolver.Resolve<IQueryProcessor>();

        private ICommandProcessor _commandProcessor;

        protected ICommandProcessor CommandProcessor
            => _commandProcessor ?? Resolver.Resolve<ICommandProcessor>();

        public BaseQueryHandler(
            IResolver resolver
        )
        {
            Resolver = resolver;
        }

        public virtual TResult Handle(TQuery Query)
        {
            return default(TResult);
        }

        protected IRepository<TEntity> GetRepository<TEntity>()
            where TEntity: class
        {
            return Resolver.Resolve<IRepository<TEntity>>();
        }
    }
}