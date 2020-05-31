namespace MAS.Payments.Models
{
    public class UpdatePaymentRequest
    {
        public long Id { get; set; }

        public double Amount { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public string Description { get; set; }

        public long PaymentTypeId { get; set; }
    }
}