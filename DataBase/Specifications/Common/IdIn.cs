namespace MAS.Payments.DataBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using MAS.Payments.Infrastructure.Specification;

    public static class CommonSpecification
    {
        public class IdIn<TEntity> : Specification<TEntity>
            where TEntity: Entity
        {
            public IEnumerable<long> Ids { get; }

            public IdIn(IEnumerable<long> keys)
            {
                Ids = [.. keys];
            }

            public IdIn(long key)
            {
                Ids = [key];
            }

            public override Expression<Func<TEntity, bool>> IsSatisfied()
                => entity => Ids.Contains(entity.Id);
        }
    }
}
