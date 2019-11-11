using System.Collections.Generic;

namespace MAS.Payments.DataBase
{
    public partial class PaymentType
    {
        public string SystemName { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public string Company { get; set; }

        public virtual User Author { get; set; }

        public long? AuthorId { get; set; }

        public virtual ICollection<MeterMeasurementType> MeasurementTypes { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }

        public PaymentType()
        {
            Payments = new List<Payment>();
            MeasurementTypes = new List<MeterMeasurementType>();
        }
    }
}