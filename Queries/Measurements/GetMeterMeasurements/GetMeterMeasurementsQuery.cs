namespace MAS.Payments.Queries
{
    using System.Collections.Generic;

    using MAS.Payments.Infrastructure.Query;

    public class GetMeterMeasurementsQuery : IQuery<IEnumerable<GetMeterMeasurementsQueryResponse>>
    {
        public byte? Month { get; }

        public int? Year { get; }

        public long? MeterMeasurementTypeId { get; }

        public GetMeterMeasurementsQuery(byte? month = null, long? metermeasurementTypeId = null, int? year = null)
        {
            Month = month;
            MeterMeasurementTypeId = metermeasurementTypeId;
            Year = year;
        }
    }
}