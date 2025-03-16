namespace MAS.Payments.Queries
{
    using System.Collections.Generic;
    using System.Linq;

    public class GetGroupedMeterMeasurementsResponse
    {
        public int DateYear { get; set; }

        public int DateMonth { get; set; }

        public IList<GetGroupedMeterMeasurementsResponseMeasurement> Measurements { get; private set; } = [];

        internal void SortMeasurements()
        {
            Measurements = Measurements.OrderBy(item => item.MeterMeasurementTypeId).ToList();
        }
    }

    public class GetGroupedMeterMeasurementsResponseMeasurement
    {
        public long Id { get; set; }

        public int DateYear { get; set; }

        public int DateMonth { get; set; }

        public double Measurement { get; set; }

        public string Comment { get; set; }

        public bool IsSent { get; set; }

        public long MeterMeasurementTypeId { get; set; }

        public string MeasurementTypeColor { get; set; }

        public string MeasurementTypeName { get; set; }
    }
}