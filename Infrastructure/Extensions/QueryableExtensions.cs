namespace MAS.Payments.Infrastructure.Extensions
{
    using System.Linq;
    using System.Threading.Tasks;

    using MAS.Payments.Infrastructure.Specification;

    using Microsoft.EntityFrameworkCore;

    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> Where<TEntity>(
            this IQueryable<TEntity> source,
            Specification<TEntity> specification
        ) => source.Where(specification.IsSatisfiedExpression);

        public static async Task<bool> Any<TEntity>(
            this IQueryable<TEntity> source,
            Specification<TEntity> specification
        ) => await source.AnyAsync(specification.IsSatisfiedExpression);

        public static async Task<int> Count<TEntity>(
            this IQueryable<TEntity> source,
            Specification<TEntity> specification
        ) => await source.CountAsync(specification.IsSatisfiedExpression);
    }
}