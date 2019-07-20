using System;
using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Exceptions;
using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Queries
{
    internal class GetEntityQueryHandler<TEntity> : BaseQueryHandler<GetEntityQuery<TEntity>, TEntity>
        where TEntity : Entity
    {
        private IRepository<TEntity> Repository { get; }
        public GetEntityQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<TEntity>();
        }

        public override TEntity Handle(GetEntityQuery<TEntity> query)
        {
            if (query.EntityId == 0)
            {
                throw new ArgumentException($"{nameof(query.EntityId)} couldn't be 0");
            }

            var entity = Repository.Get(query.EntityId);

            if (entity == null && !query.SupressException)
            {
                throw new EntityNotFoundException(typeof(TEntity), query.EntityId);
            }

            return entity;
        }
    }
}