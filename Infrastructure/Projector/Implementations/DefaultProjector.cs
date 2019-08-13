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
                    if (sourceProperty.PropertyType.IsClass)
                    {
                        var destinationComplexProperties =
                            destinationProperties
                                .Where(x => x.Name.ToLower().StartsWith(propertyName));

                        if (destinationComplexProperties.Any())
                            SetComplexProperties(source, destination, sourceProperty, destinationComplexProperties);
                    }
                    else
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
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return destination;
        }

        private void SetComplexProperties(
            TSource source, TDestination destination,
            PropertyInfo property, IEnumerable<PropertyInfo> destinationComplexProperties
        )
        {
            // todo: check

            // PaymentType.ID
            // PaymentType.Name
            // PaymentTypeID
            // PaymentTypeName

            // propertyName: PaymentType
            // destinationComplexProperties: [PaymentTypeID, PaymentTypeName]


            // ---------------
            // PaymentTypeCompanyName
            // PaymentType.Company.Name

            var sourcePropertyName = property.Name.ToLower();
            var sourcePropertyValue = property.GetValue(source);

            if (sourcePropertyValue != null)
            {
                var internalProperties = sourcePropertyValue.GetType().GetProperties();

                foreach (var internalProperty in internalProperties)
                {
                    var internalPropertyName = internalProperty.Name.ToLower();

                    if (internalProperty.PropertyType.IsClass)
                    {
                        var complexProperties =
                            destinationComplexProperties
                                .Where(x => x.Name.ToLower().StartsWith(internalPropertyName));

                        if (destinationComplexProperties.Any())
                            SetComplexProperties(source, destination, internalProperty, complexProperties);
                    }
                    else
                    {
                        var destinationPropertyNameSuggestion = $"{sourcePropertyName}{internalPropertyName}".ToLower();

                        var exactDestinationProperty =
                            destinationComplexProperties.FirstOrDefault(x => x.Name.ToLower() == destinationPropertyNameSuggestion);

                        if (exactDestinationProperty != null)
                        {
                            var internalPropertyValue = internalProperty.GetValue(source);
                            exactDestinationProperty.SetValue(destination, internalPropertyValue);
                        }
                    }


                }
            }

        }

        #endregion
    }
}