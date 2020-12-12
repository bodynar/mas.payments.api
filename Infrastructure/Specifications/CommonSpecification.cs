namespace MAS.Payments.Infrastructure.Specification
{
    using System;
    using System.Linq.Expressions;

    public class CommonSpecification<TEntity> : Specification<TEntity>
    {
        private Expression<Func<TEntity, bool>> Filter { get; }

        public CommonSpecification(Expression<Func<TEntity, bool>> filter)
        {
            Filter = filter;
        }

        public override Expression<Func<TEntity, bool>> IsSatisfied()
        {
            return Filter;
        }
    }
}