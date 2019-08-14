using System.Collections.Generic;

namespace MAS.Payments.Infrastructure.Projector
{
    public interface IProjector<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        TDestination Project(TSource src);

        IEnumerable<TDestination> Project(IEnumerable<TSource> src);
    }
}