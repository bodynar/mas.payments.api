namespace MAS.Payments.Queries
{
    public class GetPaymentTypesResponse
    {
        public long Id { get; set; }
        
        public string SystemName { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public string Company { get; set; }
    }
}