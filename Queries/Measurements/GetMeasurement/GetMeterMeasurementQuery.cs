namespace MAS.Payments.Queries
{
    using MAS.Payments.Infrastructure.Query;

    public class GetMeterMeasurementQuery : IQuery<GetMeterMeasurementResponse>
    {
        public long Id { get; }

        public GetMeterMeasurementQuery(long id)
        {
            Id = id;
        }
    }
}