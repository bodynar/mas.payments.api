namespace MAS.Payments.Queries
{
    using System;

    using MAS.Payments.Infrastructure.Query;

    public class GetMeasurementStatisticsQuery : IQuery<GetMeasurementStatisticsQueryResponse>
    {
        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public long? MeasurementTypeId { get; }

        public GetMeasurementStatisticsQuery(DateTime? from, DateTime? to, long? measurementTypeId)
        {
            From = from;
            To = to;
            MeasurementTypeId = measurementTypeId;
        }
    }
}