namespace MAS.Payments.Queries
{
    using System;
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;
    using MAS.Payments.Infrastructure.Specification;

    internal class GetMeasurementStatisticsQueryHandler : BaseQueryHandler<GetMeasurementStatisticsQuery, GetMeasurementStatisticsQueryResponse>
    {
        private IRepository<MeterMeasurement> MeasurementRepository { get; }

        public GetMeasurementStatisticsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            MeasurementRepository = GetRepository<MeterMeasurement>();
        }

        public override GetMeasurementStatisticsQueryResponse Handle(GetMeasurementStatisticsQuery query)
        {
            Specification<MeterMeasurement> measurementSpecification = new CommonSpecification<MeterMeasurement>(x => x.Date.Year == query.Year);

            if (query.MeasurementTypeId.HasValue)
            {
                measurementSpecification &= new CommonSpecification<MeterMeasurement>(x => x.MeterMeasurementTypeId == query.MeasurementTypeId);
            }

            var measurements =
                MeasurementRepository.Where(measurementSpecification)
                .OrderBy(x => x.Date)
                .ToList();

            double measurement = 0.0;

            var mappedMeasurements =
                measurements
                    .Select(x => {
                        var diff = Math.Abs(x.Measurement - measurement);
                        measurement = x.Measurement;

                        return new
                        {
                            x.Date.Month,
                            MeasurementTypeId = x.MeterMeasurementTypeId,
                            MeasurementTypeName = x.MeasurementType.Name,
                            Diff = diff
                        };
                    }).GroupBy(x => x.MeasurementTypeId);

            var response = new GetMeasurementStatisticsQueryResponse
            {
                Year = query.Year,
                TypeStatistics = mappedMeasurements.Select(x => new GetMeasurementStatisticsQueryResponse.TypeStatisticsItem
                {
                    MeasurementTypeId = x.Key,
                    MeasurementTypeName = x.First().MeasurementTypeName,
                    StatisticsData = Enumerable.Range(1, 12).Select(y => new GetMeasurementStatisticsQueryResponse.TypeStatisticsItem.StatisticsDataItem
                    {
                        Month = y,
                        Year = query.Year,
                        MeasurementDiff = x.FirstOrDefault(z => z.Month == y)?.Diff
                    })
                })
            };

            return response;
        }
    }
}
