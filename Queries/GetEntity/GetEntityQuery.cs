using MAS.Payments.DataBase;
using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Queries
{
    public class GetEntityQuery<TEntity> : IQuery<TEntity>
        where TEntity : Entity
    {
        public long EntityId { get; }

        public GetEntityQuery(long entityId)
        {
            EntityId = entityId;
        }
    }
}