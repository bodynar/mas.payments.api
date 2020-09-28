namespace MAS.Payments.Infrastructure.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.Infrastructure.Projector;
    using MAS.Payments.Infrastructure.Specification;

    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> Where<TEntity>(
            this IQueryable<TEntity> source, Specification<TEntity> specification)
        {
            return source.Where(specification.IsSatisfiedExpression);
        }

        public static bool Any<TEntity>(
            this IQueryable<TEntity> source, Specification<TEntity> specification)
        {
            return source.Any(specification.IsSatisfiedExpression);
        }

        public static int Count<TEntity>(
            this IQueryable<TEntity> source, Specification<TEntity> specification)
        {
            return source.Count(specification.IsSatisfiedExpression);
        }

        public static IEnumerable<TDestination> Project<TEntity, TDestination>(
            this IQueryable<TEntity> source, IProjector<TEntity, TDestination> projector)
            where TEntity : Entity
            where TDestination : class
        {
            return source.Select(projector.Project);
        }
    }
}