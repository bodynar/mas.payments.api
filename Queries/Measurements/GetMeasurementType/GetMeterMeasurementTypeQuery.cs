namespace MAS.Payments.Queries
{
    using MAS.Payments.Infrastructure.Query;

    public class GetMeterMeasurementTypeQuery(
        Guid id
    ) : IQuery<GetMeterMeasurementTypeResponse>
    {
        public Guid Id { get; } = id;
    }
}
