namespace MAS.Payments.Infrastructure.Specification
{
    using System;
    using System.Linq.Expressions;

    public class NotSpecification<TEntity>(
        Specification<TEntity> specification
    ) : Specification<TEntity>
    {
        public override Expression<Func<TEntity, bool>> IsSatisfied()
        {
            return Expression.Lambda<Func<TEntity, bool>>(
                Expression.Not(specification.IsSatisfiedExpression.Body),
                specification.IsSatisfiedExpression.Parameters[0]);
        }
    }
}