using System.Collections.Generic;
using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Queries
{
    public class GetMeterMeasurementsQuery : BaseUserQuery<IEnumerable<GetMeterMeasurementsResponse>>
    {
        public byte? Month { get; }

        public int? Year { get; }

        public long? MeterMeasurementTypeId { get; }

        public GetMeterMeasurementsQuery(long userId, 
            byte? month = null, long? metermeasurementTypeId = null, int? year = null)
            : base(userId)
        {
            Month = month;
            MeterMeasurementTypeId = metermeasurementTypeId;
            Year = year;
        }
    }
}