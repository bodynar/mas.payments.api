namespace MAS.Payments.Queries
{
    using System.Collections.Generic;

    using MAS.Payments.Infrastructure.Query;

    public class GetGroupedMeterMeasurementsQuery(
        byte? month = null,
        long? meterMeasurementTypeId = null,
        int? year = null
    ) : IQuery<IEnumerable<GetGroupedMeterMeasurementsResponse>>
    {
        public byte? Month { get; } = month;

        public int? Year { get; } = year;

        public long? MeterMeasurementTypeId { get; } = meterMeasurementTypeId;
    }
}