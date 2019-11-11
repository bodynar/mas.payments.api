using System;

namespace MAS.Payments.DataBase
{
    public partial class MeterMeasurement
    {
        public double Measurement { get; set; }

        public string Comment { get; set; }

        public DateTime Date { get; set; }

        public bool IsSent { get; set; }

        public virtual User Author { get; set; }

        public long? AuthorId { get; set; }

        public long MeterMeasurementTypeId { get; set; }

        public virtual MeterMeasurementType MeasurementType { get; set; }
    }
}