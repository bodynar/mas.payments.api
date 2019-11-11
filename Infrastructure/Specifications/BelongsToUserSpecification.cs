using System;
using System.Linq.Expressions;

using MAS.Payments.DataBase;

namespace MAS.Payments.Infrastructure.Specification
{
    public class BelongsToUser<TEntity> : Specification<TEntity>
        where TEntity : OwnedEntity<TEntity>
    {
        public long UserId { get; }
        
        public BelongsToUser(long userId)
        {
            UserId = userId;
        }

        public override Expression<Func<TEntity, bool>> IsSatisfied()
        {
            return x => x.GetOwnerId() == UserId;
        }
    }
}