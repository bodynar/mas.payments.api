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
        private IRepository<MeterMeasurement> MeasurementRepository { get; }

        public GetMeasurementStatisticsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            MeasurementRepository = GetRepository<MeterMeasurement>();
        }

        public override GetMeasurementStatisticsQueryResponse Handle(GetMeasurementStatisticsQuery query)
        {
            var measurements =
                MeasurementRepository.Where(
                    new CommonSpecification<MeterMeasurement>(x =>
                        x.MeterMeasurementTypeId == query.MeasurementTypeId
                        && x.Date.Year == query.Year
                    )
                )
                .OrderBy(x => x.Date)
                .ToList();

            double measurement = 0.0;

            var mappedMeasurements =
                measurements.Select(x => {
                    var diff = Math.Abs(x.Measurement - measurement);
                    measurement = x.Measurement;

                    return new 
                    {
                        Month = x.Date.Month,
                        Diff = diff
                    };
                }).ToDictionary(x => x.Month, x => x.Diff as double?);

            var response = new GetMeasurementStatisticsQueryResponse
            {
                Year = query.Year,
                MeasurementTypeId = query.MeasurementTypeId,
                StatisticsData =
                    Enumerable.Range(1, 12).Select(x => new GetMeasurementStatisticsQueryResponse.StatisticsDataItem
                    {
                        Month = x,
                        Year = query.Year,
                        MeasurementDiff = mappedMeasurements.GetValueOrDefault(x, null)
                    })
            };

            return response;
        }
    }
}
