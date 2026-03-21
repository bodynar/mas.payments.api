namespace MAS.Payments.Queries
{
    public class GetMeterMeasurementTypeResponse
    {
        public Guid Id { get; set; }
        
        public string SystemName { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public Guid PaymentTypeId { get; set; }

        public string PaymentTypeName { get; set; }

        public string Color { get; set; }
    }
}
