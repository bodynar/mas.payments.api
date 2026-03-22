namespace MAS.Payments.Models
{
    public class UpdateMeterMeasurementTypeRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid PaymentTypeId { get; set; }

        public string Color { get; set; }
    }
}
