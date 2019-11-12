using System;

namespace MAS.Payments.Models
{
    public class GetStatisticsRequest
    {
        public bool IncludeMeasurements { get; set; }

        public int? Year { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public bool IsDatePeriodSpecified
            => From.HasValue && To.HasValue
            && From.Value < To.Value;
    }
}