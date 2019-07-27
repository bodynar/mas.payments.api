using System.Collections.Generic;
using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Queries
{
    public class GetMeterMeasurementsQuery : IQuery<IEnumerable<GetMeterMeasurementsResponse>>
    {
        public byte? Month { get; }

        public long? MeterMeasurementTypeId { get; }

        public GetMeterMeasurementsQuery(byte? month = null, long? metermeasurementTypeId = null)
        {
            Month = month;
            MeterMeasurementTypeId = metermeasurementTypeId;
        }
    }
}