using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Queries
{
    public class GetMeasurementTypeQuery : IQuery<GetMeterMeasurementTypeResponse>
    {
        public long Id { get; }

        public GetMeasurementTypeQuery(long id)
        {
            Id = id;
        }
    }
}