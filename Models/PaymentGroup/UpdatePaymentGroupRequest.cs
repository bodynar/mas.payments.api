namespace MAS.Payments.Models
{
    using System;

    public class UpdatePaymentGroupRequest
    {
        public Guid Id { get; set; }

        public DateTime PaymentDate { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public string Comment { get; set; }
    }
}
