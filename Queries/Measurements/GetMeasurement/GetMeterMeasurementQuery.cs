using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Queries
{
    public class GetMeterMeasurementQuery : IQuery<GetMeterMeasurementResponse>
    {
        public long Id { get; }

        public GetMeterMeasurementQuery(long id)
        {
            Id = id;
        }
    }
}