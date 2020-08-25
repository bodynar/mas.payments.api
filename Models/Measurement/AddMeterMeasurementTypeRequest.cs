using System;

namespace MAS.Payments.Models
{
    public class AddMeterMeasurementTypeRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public long PaymentTypeId { get; set; }

        public string Color { get; set; }
    }
}