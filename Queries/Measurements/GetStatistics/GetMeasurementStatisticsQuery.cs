namespace MAS.Payments.Queries
{
    using System;

    using MAS.Payments.Infrastructure.Query;

    public class GetMeasurementStatisticsQuery(
        DateTime? from,
        DateTime? to,
        long? measurementTypeId
    ) : IQuery<GetMeasurementStatisticsQueryResponse>
    {
        public DateTime? From { get; set; } = from;

        public DateTime? To { get; set; } = to;

        public long? MeasurementTypeId { get; } = measurementTypeId;
    }
}