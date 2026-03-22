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
            public IEnumerable<Guid> Ids { get; }

            public IdIn(IEnumerable<Guid> keys)
            {
                Ids = [.. keys];
            }

            public IdIn(Guid key)
            {
                Ids = [key];
            }

            public override Expression<Func<TEntity, bool>> IsSatisfied()
                => entity => Ids.Contains(entity.Id);
        }
    }
}
