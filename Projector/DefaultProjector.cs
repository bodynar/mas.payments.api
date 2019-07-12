using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

            var destinationTypeHasComplexProperties = CheckIfDestinationIsComplex();

            if (destinationTypeHasComplexProperties)
            {
                throw new NotSupportedException($"Type {DestinationType.Name} contains some complex properties. Current mapper doesn't support mapping to complex properties.");
            }
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

        private bool CheckIfDestinationIsComplex()
        {
            return DestinationType.GetProperties().Any(x => !x.PropertyType.IsPrimitive);
        }

        private TDestination SetValues(TSource source)
        {
            // only primitives

            var sourceProperties = SourceType.GetProperties();
            var destinationProperties = DestinationType.GetProperties();

            TDestination destination = Activator.CreateInstance<TDestination>();

            foreach (var sourceProperty in sourceProperties)
            {
                var propertyName = sourceProperty.Name.ToLower();

                try
                {
                    var destinationProperty =
                            destinationProperties
                                .FirstOrDefault(x =>
                                    x.Name.ToLower() == propertyName
                                    && x.PropertyType == sourceProperty.PropertyType);

                    if (destinationProperty != null)
                    {
                        var sourceValue = sourceProperty.GetValue(source);
                        destinationProperty.SetValue(destination, sourceValue);
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return destination;
        }

        #endregion
    }
}