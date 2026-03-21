namespace MAS.Payments.Queries
{
    using MAS.Payments.Infrastructure.Query;

    public class GetMeterMeasurementQuery(
        Guid id
    ) : IQuery<GetMeterMeasurementResponse>
    {
        public Guid Id { get; } = id;
    }
}
