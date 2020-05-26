using System.Collections.Generic;
using System.Linq;

namespace MAS.Payments.Queries
{
    public class GetMeterMeasurementsResponse
    {
        public int DateYear { get; set; }

        public int DateMonth { get; set; }

        public IList<GetMeterMeasurementsResponseMeasurement> Measurements { get; private set; }

        public GetMeterMeasurementsResponse()
        {
            Measurements = new List<GetMeterMeasurementsResponseMeasurement>();
        }

        internal void SortMeasurements()
        {
            Measurements = Measurements.OrderBy(item => item.MeterMeasurementTypeId).ToList();
        }
    }

    public class GetMeterMeasurementsResponseMeasurement
    {
        public long Id { get; set; }

        public double Measurement { get; set; }

        public string Comment { get; set; }

        public bool IsSent { get; set; }

        public long MeterMeasurementTypeId { get; set; }

        public string MeasurementTypeName { get; set; }
    }
}