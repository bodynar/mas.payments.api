namespace MAS.Payments.Models
{
    using System;
    using System.Collections.Generic;

    public class AddPaymentGroupRequest
    {
        public DateTime Date { get; set; }

        public IEnumerable<PaymentGroupRequestModel> Payments { get; set; }
    }

    public class PaymentGroupRequestModel
    {
        public double Amount { get; set; }

        public string Description { get; set; }

        public long PaymentTypeId { get; set; }
    }
}
