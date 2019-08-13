using System;

namespace MAS.Payments.Models
{
    public class AddMeterMeasurementRequest
    {
        public double Measurement { get; set; }

        public string Comment { get; set; }

        public DateTime Date { get; set; }

        public long MeterMeasurementTypeId { get; set; }
    }
}