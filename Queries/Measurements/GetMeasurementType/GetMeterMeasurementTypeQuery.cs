using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Queries
{
    public class GetMeterMeasurementTypeQuery : IQuery<GetMeterMeasurementTypeResponse>
    {
        public long Id { get; }

        public GetMeterMeasurementTypeQuery(long id)
        {
            Id = id;
        }
    }
}