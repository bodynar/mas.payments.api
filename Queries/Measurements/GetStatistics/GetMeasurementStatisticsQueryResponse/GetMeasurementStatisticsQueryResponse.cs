namespace MAS.Payments.Queries
{
    using System;
    using System.Collections.Generic;

    public class GetMeasurementStatisticsQueryResponse
    {
        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public ICollection<MeasurementTypeStatisticsItem> TypeStatistics { get; } = new List<MeasurementTypeStatisticsItem>();
    }
}