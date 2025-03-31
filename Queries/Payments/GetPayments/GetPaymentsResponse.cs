namespace MAS.Payments.Queries
{
    public class GetPaymentsResponse
    {
        public long Id { get; set; }

        public double Amount { get; set; }

        public int DateYear { get; set; }

        public int DateMonth { get; set; }

        public string Description { get; set; }

        public string PaymentTypeName { get; set; }
        
        public string PaymentTypeColor { get; set; }

        public long PaymentTypeId { get; set; }

        public bool HasReceipt { get; set; }

        public bool HasCheck { get; set; }
    }
}