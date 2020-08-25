using System.Collections.Generic;

namespace MAS.Payments.DataBase
{
    public class MeterMeasurementType : Entity
    {
        public string SystemName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long PaymentTypeId { get; set; }

        public string Color { get; set; }

        public virtual PaymentType PaymentType { get; set; }

        public virtual ICollection<MeterMeasurement> MeterMeasurements { get; }

        public MeterMeasurementType()
        {
            MeterMeasurements = new List<MeterMeasurement>();
        }
    }
}