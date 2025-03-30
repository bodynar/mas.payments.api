namespace MAS.Payments.Models
{
    using Microsoft.AspNetCore.Http;

    public class AddPaymentRequest
    {
        public double Amount { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public string Description { get; set; }

        public long PaymentTypeId { get; set; }

        public IFormFile ReceiptFile { get; set; }

        public IFormFile CheckFile { get; set; }
    }
}