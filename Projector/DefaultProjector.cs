using System;
using System.Collections.Generic;
using System.Linq;

namespace MAS.Payments.Projector
{
    internal class DefaultProjector<TSource, TDestination> : IProjector<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        private Type SourceType { get; }

        private Type DestinationType { get; }

        internal DefaultProjector()
        {
            SourceType = typeof(TSource);
            DestinationType = typeof(TDestination);
        }

        public TDestination Project(TSource src)
        {
            var dest = SetValues(src);

            return dest;
        }

        public IEnumerable<TDestination> Project(IEnumerable<TSource> src)
        {
            foreach (var item in src)
                yield return Project(item);
        }

        #region Not public API

        private TDestination SetValues(TSource src)
        {
            // only primitives

            var srcProps = SourceType.GetProperties().Where(x => x.PropertyType.IsPrimitive);
            var destProprs = DestinationType.GetProperties().Where(x => x.PropertyType.IsPrimitive);

            TDestination dest = Activator.CreateInstance<TDestination>();

            foreach (var destProperty in destProprs)
            {
                var propName = destProperty.Name.ToLower();

                try
                {
                    var srcProperty =
                        srcProps
                            .FirstOrDefault(x =>
                                x.Name.ToLower() == propName
                                && x.PropertyType == destProperty.PropertyType);

                    if (srcProperty != null)
                    {
                        var srcValue = srcProperty.GetValue(src);
                        destProperty.SetValue(dest, srcValue);
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return dest;
        }

        #endregion
    }
}