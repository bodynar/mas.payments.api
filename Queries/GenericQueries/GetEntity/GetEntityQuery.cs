using MAS.Payments.DataBase;
using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Queries
{
    public class GetEntityQuery<TEntity> : IQuery<TEntity>
        where TEntity : Entity
    {
        public long EntityId { get; }

        public bool SupressException { get; }

        public GetEntityQuery(long entityId, bool supressException = true)
        {
            EntityId = entityId;
            SupressException = supressException;
        }
    }
}