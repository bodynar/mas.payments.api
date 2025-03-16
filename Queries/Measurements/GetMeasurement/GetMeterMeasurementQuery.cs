namespace MAS.Payments.Queries
{
    using MAS.Payments.Infrastructure.Query;

    public class GetMeterMeasurementQuery(
        long id
    ) : IQuery<GetMeterMeasurementResponse>
    {
        public long Id { get; } = id;
    }
}