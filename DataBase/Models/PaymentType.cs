namespace MAS.Payments.DataBase
{
    using System.Collections.Generic;

    public class PaymentType : Entity
    {
        public string SystemName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Company { get; set; }

        public string Color { get; set; }

        public virtual ICollection<MeterMeasurementType> MeasurementTypes { get; } = [];

        public virtual ICollection<Payment> Payments { get; } = [];
    }
}