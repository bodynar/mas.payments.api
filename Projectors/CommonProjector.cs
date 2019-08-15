using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using MAS.Payments.Infrastructure.Projector;

namespace MAS.Payments.Projectors
{
    public partial class Projector
    {
        internal class Common<TSource, TDestination> : IProjector<TSource, TDestination>
            where TSource : class
            where TDestination : class
        {
            private Func<TSource, TDestination> ProjectFuntion
            {
                get
                {
                    if (ProjectExpression == null)
                    {
                        throw new Exception($"Projector function isn't implemented");
                    }

                    return ProjectExpression.Compile();
                }
            }

            private Expression<Func<TSource, TDestination>> ProjectExpression { get; }

            public Common(Expression<Func<TSource, TDestination>> projectExpression)
            {
                ProjectExpression = projectExpression
                    ?? throw new ArgumentException("Project expression is required");
            }

            public TDestination Project(TSource src)
            {
                return ProjectFuntion(src);
            }

            public IEnumerable<TDestination> Project(IEnumerable<TSource> src)
            {
                foreach (var item in src)
                    yield return ProjectFuntion(item);
            }
        }
    }
}