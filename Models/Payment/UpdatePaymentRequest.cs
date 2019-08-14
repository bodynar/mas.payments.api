using System;

namespace MAS.Payments.Models
{
    public class UpdatePaymentRequest
    {
        public long Id { get; set; }

        public double Amount { get; set; }

        public DateTime? Date { get; set; }

        public string Description { get; set; }

        public long PaymentTypeId { get; set; }
    }
}