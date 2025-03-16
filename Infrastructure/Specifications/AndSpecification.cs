namespace MAS.Payments.Infrastructure.Specification
{
    using System;
    using System.Linq.Expressions;

    using MAS.Payments.Infrastructure.Extensions;

    public class AndSpecification<TEntity>(
        Specification<TEntity> left,
        Specification<TEntity> right
    ) : Specification<TEntity>
    {
        public override Expression<Func<TEntity, bool>> IsSatisfied()
            => left.IsSatisfiedExpression.Combine(
                right.IsSatisfiedExpression, 
                Expression.AndAlso);
    }
}