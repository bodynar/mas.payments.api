using System;
using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Infrastructure.Query
{
    internal abstract class BaseQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
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

        public BaseQueryHandler(
            IResolver resolver
        )
        {
            Resolver = resolver;
        }

        public abstract TResult Handle(TQuery query);

        protected IRepository<TEntity> GetRepository<TEntity>()
            where TEntity: Entity
        {
            return Resolver.Resolve<IRepository<TEntity>>();
        }
    }
}