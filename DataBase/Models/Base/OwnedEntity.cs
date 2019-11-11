using System;
using System.Linq.Expressions;

namespace MAS.Payments.DataBase
{
    public abstract class OwnedEntity<TEntity> : Entity
        where TEntity : OwnedEntity<TEntity>
    {
        public abstract Expression<Func<TEntity, long?>> EntityOwnerId { get; }

        public long? GetOwnerId()
        {
            return EntityOwnerId.Compile().Invoke((TEntity)this);
        }
    }
}