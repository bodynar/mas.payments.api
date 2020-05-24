using System.Collections.Generic;

namespace MAS.Payments.Queries
{
    public class GetMeterMeasurementsResponse
    {
        public int DateYear { get; set; }

        public int DateMonth { get; set; }

        public IList<GetMeterMeasurementsResponseMeasurement> Measurements { get; }

        public GetMeterMeasurementsResponse()
        {
            Measurements = new List<GetMeterMeasurementsResponseMeasurement>();
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