namespace MAS.Payments.Queries
{
    using MAS.Payments.Infrastructure.Query;

    public class GetMeterMeasurementTypeQuery : IQuery<GetMeterMeasurementTypeResponse>
    {
        public long Id { get; }

        public GetMeterMeasurementTypeQuery(long id)
        {
            Id = id;
        }
    }
}