namespace MAS.Payments.Queries
{
    public class GetMeterMeasurementTypesResponse
    {
        public long Id { get; set; }

        public string SystemName { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public long PaymentTypeId { get; set; }

        public string PaymentTypeName { get; set; }

        public bool HasRelatedMeasurements { get; set; }
    }
}