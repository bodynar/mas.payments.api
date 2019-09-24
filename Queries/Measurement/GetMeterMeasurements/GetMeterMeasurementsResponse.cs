using System;

namespace MAS.Payments.Queries
{
    public class GetMeterMeasurementsResponse
    {
        public long Id { get; set; }

        public double Measurement { get; set; }

        public string Comment { get; set; }

        public DateTime Date { get; set; }

        public bool IsSent { get; set; }

        public long MeterMeasurementTypeId { get; set; }

        public string MeasurementTypeName { get; set; }
    }
}