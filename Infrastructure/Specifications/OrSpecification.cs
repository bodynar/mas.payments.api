using System;
using System.Linq.Expressions;
using MAS.Payments.Infrastructure.Extensions;

namespace MAS.Payments.Infrastructure.Specification
{
    public class OrSpecification<TEntity> : Specification<TEntity>
    {
        private Specification<TEntity> LeftSpecification { get; }

        private Specification<TEntity> RightSpecification { get; }

        public OrSpecification(Specification<TEntity> left, Specification<TEntity> right)
        {
            LeftSpecification = left;
            RightSpecification = right;
        }

        public override Expression<Func<TEntity, bool>> IsSatisfied()
            => LeftSpecification.IsSatisfiedExpression.Combine(
                RightSpecification.IsSatisfiedExpression, 
                Expression.OrElse);
    }
}