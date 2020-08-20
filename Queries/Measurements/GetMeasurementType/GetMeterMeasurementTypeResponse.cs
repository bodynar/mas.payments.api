namespace MAS.Payments.Queries
{
    public class GetMeterMeasurementTypeResponse
    {
        public long Id { get; set; }
        
        public string SystemName { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public long PaymentTypeId { get; set; }

        public string PaymentTypeName { get; set; }

        public string Color { get; set; }
    }
}