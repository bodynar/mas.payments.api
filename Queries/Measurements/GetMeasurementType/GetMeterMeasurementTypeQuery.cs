namespace MAS.Payments.Queries
{
    using MAS.Payments.Infrastructure.Query;

    public class GetMeterMeasurementTypeQuery(
        long id
    ) : IQuery<GetMeterMeasurementTypeResponse>
    {
        public long Id { get; } = id;
    }
}