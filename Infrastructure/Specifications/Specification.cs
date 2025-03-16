namespace MAS.Payments.Infrastructure.Specification
{
    using System;
    using System.Linq.Expressions;

    public abstract class Specification<TEntity>
    {
        public static Specification<TEntity> operator &(Specification<TEntity> left, Specification<TEntity> right)
            => new AndSpecification<TEntity>(left, right);

        public static Specification<TEntity> operator |(Specification<TEntity> left, Specification<TEntity> right)
            => new OrSpecification<TEntity>(left, right);

        public static Specification<TEntity> operator !(Specification<TEntity> specification)
            => new NotSpecification<TEntity>(specification);

        public static bool operator true(Specification<TEntity> specification)
            => false;

        public static bool operator false(Specification<TEntity> specification)
            => false;

        public abstract Expression<Func<TEntity, bool>> IsSatisfied();

        public Expression<Func<TEntity, bool>> IsSatisfiedExpression
        {
            get
            {
                var expression = IsSatisfied();
                return expression ?? throw new NotImplementedException($"Specification {GetType()} isn't implemented");
            }
        }

        public Func<TEntity, bool> IsSatisfiedFunc
        {
            get
            {
                var expression = IsSatisfied();
                return expression == null ? throw new NotImplementedException($"Specification {GetType()} isn't implemented") : expression.Compile();
            }
        }
    }
}