using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Queries
{
    public class GetMeasurementQuery : IQuery<GetMeterMeasurementResponse>
    {
        public long Id { get; }

        public GetMeasurementQuery(long id)
        {
            Id = id;
        }
    }
}