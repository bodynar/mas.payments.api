namespace MAS.Payments.DataBase
{
    using System;

    public class MeterMeasurement : Entity
    {
        public double Measurement { get; set; }

        public string Comment { get; set; }

        public DateTime Date { get; set; }

        public bool IsSent { get; set; }

        public long MeterMeasurementTypeId { get; set; }

        public virtual MeterMeasurementType MeasurementType { get; set; }
    }
}