using MAS.Payments.Projector;

namespace MAS.Payments.Infrastructure.Extensions
{
    public static class ObjectExtenstions
    {
        public static TDestination Project<TSource, TDestination>(
            this TSource source, IProjector<TSource, TDestination> projector)
            where TSource : class
            where TDestination : class
        {
            return projector.Project(source);
        }
    }
}