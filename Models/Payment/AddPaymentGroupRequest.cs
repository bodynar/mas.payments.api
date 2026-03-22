namespace MAS.Payments.Models
{
    using System;
    using System.Collections.Generic;

    public class AddPaymentGroupRequest
    {
        public DateTime PaymentDate { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public string Comment { get; set; }

        public IEnumerable<PaymentGroupRequestModel> Payments { get; set; }
    }

    public class PaymentGroupRequestModel
    {
        public double Amount { get; set; }

        public string Description { get; set; }

        public Guid PaymentTypeId { get; set; }
    }
}
