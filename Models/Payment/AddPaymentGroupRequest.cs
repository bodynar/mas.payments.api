namespace MAS.Payments.Models
{
    using Microsoft.AspNetCore.Http;

    public class AddPaymentGroupRequest
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public string Payments { get; set; }
    }

    public class PaymentGroupRequestModel
    {
        public double Amount { get; set; }

        public string Description { get; set; }

        public long PaymentTypeId { get; set; }
    }
}
