using System;
using System.Linq.Expressions;

namespace MAS.Payments.Infrastructure.Specification
{
    public class NotSpecification<TEntity> : Specification<TEntity>
    {
        private Specification<TEntity> Specification { get; }

        public NotSpecification(Specification<TEntity> specification)
        {
            Specification = specification;
        }

        public override Expression<Func<TEntity, bool>> IsSatisfied()
        {
            return Expression.Lambda<Func<TEntity, bool>>(
                Expression.Not(Specification.IsSatisfiedExpression.Body),
                Specification.IsSatisfiedExpression.Parameters[0]);
        }
    }
}