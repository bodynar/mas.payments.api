namespace MAS.Payments.Infrastructure.Specification
{
    using System;
    using System.Linq.Expressions;

    using MAS.Payments.Infrastructure.Extensions;

    public class AndSpecification<TEntity> : Specification<TEntity>
    {
        private Specification<TEntity> LeftSpecification { get; }

        private Specification<TEntity> RightSpecification { get; }

        public AndSpecification(Specification<TEntity> left, Specification<TEntity> right)
        {
            LeftSpecification = left;
            RightSpecification = right;
        }

        public override Expression<Func<TEntity, bool>> IsSatisfied()
            => LeftSpecification.IsSatisfiedExpression.Combine(
                RightSpecification.IsSatisfiedExpression, 
                Expression.AndAlso);
    }
}