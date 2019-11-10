using System.Collections.Generic;

namespace MAS.Payments.DataBase
{
    public partial class MeterMeasurementType : Entity
    {
        public string SystemName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long PaymentTypeId { get; set; }

        public virtual User Author { get; set; }

        public virtual PaymentType PaymentType { get; set; }

        public virtual ICollection<MeterMeasurement> MeterMeasurements { get; set; }

        public MeterMeasurementType()
        {
            MeterMeasurements = new List<MeterMeasurement>();
        }
    }
}