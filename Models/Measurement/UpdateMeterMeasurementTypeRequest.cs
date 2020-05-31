namespace MAS.Payments.Models
{
    public class UpdateMeterMeasurementTypeRequest
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long PaymentTypeId { get; set; }
    }
}