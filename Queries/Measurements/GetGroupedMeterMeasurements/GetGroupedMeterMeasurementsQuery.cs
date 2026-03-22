namespace MAS.Payments.Queries
{
    using System.Collections.Generic;

    using MAS.Payments.Infrastructure.Query;

    public class GetGroupedMeterMeasurementsQuery(
        byte? month = null,
        Guid? meterMeasurementTypeId = null,
        int? year = null
    ) : IQuery<IEnumerable<GetGroupedMeterMeasurementsResponse>>
    {
        public byte? Month { get; } = month;

        public int? Year { get; } = year;

        public Guid? MeterMeasurementTypeId { get; } = meterMeasurementTypeId;
    }
}
