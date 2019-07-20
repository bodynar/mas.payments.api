using System;

namespace MAS.Payments.DataBase
{
    public class MeterMeasurement : Entity
    {
        public double Measurement { get; set; }

        public string Comment { get; set; }

        public DateTime Date { get; set; }

        public long MeterMeasurementTypeId { get; set; }

        public MeterMeasurementType MeasurementType { get; set; }
    }
}