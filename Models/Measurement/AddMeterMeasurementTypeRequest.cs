namespace MAS.Payments.Models
{
    using System;

    public class AddMeterMeasurementTypeRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public long PaymentTypeId { get; set; }

        public string Color { get; set; }
    }
}