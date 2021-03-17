namespace MAS.Payments.Models
{
    using System;

    public class GetMeasurementStatisticsRequest
    {
        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public long? MeasurementTypeId { get; set; }
    }
}