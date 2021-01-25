namespace MAS.Payments.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;
    using MAS.Payments.Infrastructure.Specification;

    internal class GetMeasurementStatisticsQueryHandler : BaseQueryHandler<GetMeasurementStatisticsQuery, GetMeasurementStatisticsQueryResponse>
    {
        private static readonly IEnumerable<int> MonthNumbers =
            Enumerable.Range(1, 12);

        private IRepository<MeterMeasurement> MeasurementRepository { get; }

        public GetMeasurementStatisticsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            MeasurementRepository = GetRepository<MeterMeasurement>();
        }

        public override GetMeasurementStatisticsQueryResponse Handle(GetMeasurementStatisticsQuery query)
        {
            if (query.From.HasValue && query.To.HasValue && query.From.Value <= query.To.Value)
            {
                throw new ArgumentException($"${nameof(query.From)} value cannot be less or equal than {nameof(query.To)}.");
            }

            Specification<MeterMeasurement> filter = new CommonSpecification<MeterMeasurement>(x => true);

            if (query.MeasurementTypeId.HasValue)
            {
                filter &= new CommonSpecification<MeterMeasurement>(x => x.MeterMeasurementTypeId == query.MeasurementTypeId);
            }
            if (query.From.HasValue)
            {
                filter &= new CommonSpecification<MeterMeasurement>(x => x.Date.Date >= query.From.Value.Date);
            }
            if (query.To.HasValue)
            {
                filter &= new CommonSpecification<MeterMeasurement>(x => x.Date.Date <= query.To.Value.Date);
            }

            var measurements =
                MeasurementRepository.Where(filter)
                .ToList()
                .GroupBy(x => x.MeterMeasurementTypeId);

            var response = new GetMeasurementStatisticsQueryResponse()
            {
                From = query.From,
                To = query.To
            };

            foreach (var typeGroup in measurements)
            {
                var firstItem = typeGroup.FirstOrDefault();

                if (firstItem == null)
                {
                    continue;
                }

                var typeStatisticsItem = new TypeStatisticsItem()
                {
                    MeasurementTypeId = typeGroup.Key,
                    MeasurementTypeName = firstItem.MeasurementType.Name
                };

                var groupedByYear = typeGroup.GroupBy(x => x.Date.Year).OrderBy(x => x.Key);

                double previousMeasurement = 0.0;

                foreach (var yearGroup in groupedByYear)
                {
                    foreach (var monthNumber in MonthNumbers)
                    {
                        var item = yearGroup.FirstOrDefault(x => x.Date.Month == monthNumber);

                        typeStatisticsItem.StatisticsData.Add(new StatisticsDataItem
                        {
                            Year = yearGroup.Key,
                            Month = monthNumber,
                            MeasurementDiff = item == null ? null : (double?)Math.Abs(item.Measurement - previousMeasurement)
                        });

                        if (item != null)
                        {
                            previousMeasurement = item.Measurement;
                        }
                    }
                }

                response.TypeStatistics.Add(typeStatisticsItem);
            }

            return response;
        }
    }
}
