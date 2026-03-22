namespace MAS.Payments.Models
{
    using System;

    public class GetPaymentsStatisticsRequest
    {
        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public Guid? PaymentTypeId { get; set; }
    }
}
