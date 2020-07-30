namespace MAS.Payments.Projectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using MAS.Payments.Infrastructure.Projector;

    public static partial class Projector
    {
        internal class ToFlat<TSource, TDestination> : IProjector<TSource, TDestination>
            where TSource : class
            where TDestination : class
        {
            private Type DestinationType { get; }

            private Type[] PrimitiveTypes { get; }

            internal ToFlat()
            {
                DestinationType = typeof(TDestination);
                PrimitiveTypes = FulfillPrimitiveTypes();

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
                return DestinationType
                        .GetProperties()
                        .Any(x =>
                            x.PropertyType.IsClass
                            && !PrimitiveTypes.Contains(x.PropertyType));
            }

            private TDestination SetValues(TSource source)
            {
                var sourceProperties = typeof(TSource).GetProperties();
                var destinationProperties = DestinationType.GetProperties();

                TDestination destination = Activator.CreateInstance<TDestination>();

                foreach (var sourceProperty in sourceProperties)
                {
                    var propertyName = sourceProperty.Name.ToLower();

                    try
                    {
                        if ((sourceProperty.PropertyType.IsClass || sourceProperty.PropertyType.IsValueType)
                            && !PrimitiveTypes.Contains(sourceProperty.PropertyType))
                        {
                            var destinationComplexProperties =
                                destinationProperties
                                    .Where(x => x.Name.ToLower().StartsWith(propertyName));

                            if (destinationComplexProperties.Any())
                                SetComplexProperties(source, destination, sourceProperty, destinationComplexProperties, 1);
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
                object source, TDestination destination,
                PropertyInfo property, IEnumerable<PropertyInfo> destinationComplexProperties,
                int deepLevel
            )
            {
                if (deepLevel >= 5) return;

                var sourcePropertyName = property.Name.ToLower();
                var sourcePropertyValue = property.GetValue(source);

                if (sourcePropertyValue != null)
                {
                    var internalProperties = sourcePropertyValue.GetType().GetProperties();

                    foreach (var internalProperty in internalProperties)
                    {
                        var internalPropertyName = internalProperty.Name.ToLower();

                        if ((internalProperty.PropertyType.IsClass || internalProperty.PropertyType.IsValueType)
                            && !PrimitiveTypes.Contains(internalProperty.PropertyType))
                        {
                            var complexProperties =
                                destinationComplexProperties
                                    .Where(x => x.Name.ToLower().StartsWith(internalPropertyName));

                            if (destinationComplexProperties.Any())
                                SetComplexProperties(sourcePropertyValue, destination, internalProperty, complexProperties, deepLevel + 1);
                        }
                        else
                        {
                            var destinationPropertyNameSuggestion = $"{sourcePropertyName}{internalPropertyName}".ToLower();

                            var exactDestinationProperty =
                                destinationComplexProperties.FirstOrDefault(x => x.Name.ToLower() == destinationPropertyNameSuggestion);

                            if (exactDestinationProperty != null)
                            {
                                var internalPropertyValue = internalProperty.GetValue(sourcePropertyValue);
                                exactDestinationProperty.SetValue(destination, internalPropertyValue);
                            }
                        }


                    }
                }

            }

            private Type[] FulfillPrimitiveTypes()
            {
                return new[] {
                    typeof(bool),
                    typeof(string),
                    typeof(char),
                    typeof(byte),
                    typeof(sbyte),
                    typeof(ushort),
                    typeof(short),
                    typeof(uint),
                    typeof(int),
                    typeof(ulong),
                    typeof(long),
                    typeof(float),
                    typeof(double),
                    typeof(decimal),
                    typeof(DateTime),
                };
            }

            #endregion
        }
    }
}