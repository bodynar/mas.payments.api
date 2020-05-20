namespace MAS.Payments.Queries
{
    public class GetPaymentResponse
    {
        public long Id { get; set; }

        public double Amount { get; set; }

        public int DateYear { get; set; }

        public int DateMonth { get; set; }

        public string Description { get; set; }

        public string PaymentType { get; set; }

        public long PaymentTypeId { get; set; }
    }
}