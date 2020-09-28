namespace MAS.Payments.Infrastructure.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    using MAS.Payments.Infrastructure.Projector;
    using MAS.Payments.Infrastructure.Specification;

    public static class EnumerableExtensions
    {
        public static IEnumerable<TSource> Where<TSource>(
            this IEnumerable<TSource> source, Specification<TSource> specification)
        {
            return source?.Where(specification.IsSatisfiedFunc);
        }

        public static bool Any<TSource>(
            this IEnumerable<TSource> source, Specification<TSource> specification)
        {
            return source.Any(specification.IsSatisfiedFunc);
        }

        public static bool All<TSource>(
            this IEnumerable<TSource> source, Specification<TSource> specification)
        {
            return source.All(specification.IsSatisfiedFunc);
        }

        public static IEnumerable<TDestination> Project<TSource, TDestination>(
            this IEnumerable<TSource> source, IProjector<TSource, TDestination> projector)
            where TSource : class
            where TDestination : class
        {
            return source.Select(projector.Project);
        }
    }
}