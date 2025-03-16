namespace MAS.Payments.Infrastructure.Specification
{
    using System;
    using System.Linq.Expressions;

    public class CommonSpecification<TEntity>(
        Expression<Func<TEntity, bool>> filter
    ) : Specification<TEntity>
    {
        public override Expression<Func<TEntity, bool>> IsSatisfied()
        {
            return filter;
        }
    }
}