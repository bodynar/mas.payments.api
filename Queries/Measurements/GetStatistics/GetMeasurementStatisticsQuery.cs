namespace MAS.Payments.Queries
{
    using MAS.Payments.Infrastructure.Query;

    public class GetMeasurementStatisticsQuery : IQuery<GetMeasurementStatisticsQueryResponse>
    {
        public short Year { get; }

        public long MeasurementTypeId { get; }

        public GetMeasurementStatisticsQuery(short year, long measurementTypeId)
        {
            Year = year;
            MeasurementTypeId = measurementTypeId;
        }
    }
}
