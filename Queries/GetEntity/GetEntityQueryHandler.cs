using MAS.Payments.DataBase;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Queries
{
    internal class GetEntityQueryHandler<TEntity> : BaseQueryHandler<GetEntityQuery<TEntity>, TEntity>
        where TEntity : Entity
    {
        public GetEntityQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
        }

        public override TEntity Handle(GetEntityQuery<TEntity> query)
        {
            return GetRepository<TEntity>().Get(query.EntityId);
        }
    }
}