using System;

using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Queries
{
    public class GetStatisticsQuery : BaseUserQuery<GetStatisticsResponse>
    {
        public bool IncludeMeasurements { get; }

        public bool IsDatePeriodSpecified
            => From.HasValue && To.HasValue;

        public int? Year { get; }

        public DateTime? From { get; }

        public DateTime? To { get; }

        public GetStatisticsQuery(long userId, bool includeMeasurements = false)
            : base(userId)
        {
            IncludeMeasurements = includeMeasurements;
        }

        public GetStatisticsQuery(long userId, int year, bool includeMeasurements = false)
            : base(userId)
        {
            Year = year;
            IncludeMeasurements = includeMeasurements;
        }

        public GetStatisticsQuery(long userId, DateTime from, DateTime to, bool includeMeasurements = false)
            : base(userId)
        {
            From = from;
            To = to;
            IncludeMeasurements = includeMeasurements;
        }
    }
}