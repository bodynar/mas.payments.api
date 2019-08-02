using System;
using System.Collections.Generic;
using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Queries
{
    public class GetStatisticsQuery : IQuery<IEnumerable<GetStatisticsResponse>>
    {
        public bool IncludeMeasurements { get; }

        public bool IsDatePeriodSpecified
            => From.HasValue && To.HasValue;

        public int? Year { get; }

        public DateTime? From { get; }

        public DateTime? To { get; }

        public GetStatisticsQuery(bool includeMeasurements = false)
        {
            IncludeMeasurements = includeMeasurements;
        }

        public GetStatisticsQuery(int year, bool includeMeasurements = false)
        {
            Year = year;
            IncludeMeasurements = includeMeasurements;
        }

        public GetStatisticsQuery(DateTime from, DateTime to, bool includeMeasurements = false)
        {
            From = from;
            To = to;
            IncludeMeasurements = includeMeasurements;
        }
    }
}