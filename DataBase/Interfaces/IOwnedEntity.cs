using System;
using System.Linq.Expressions;

namespace MAS.Payments.DataBase
{
    public interface IOwnedEntity<TEntity>
    {
        Expression<Func<TEntity, long>> EntityOwnerId { get; }
    }
}