namespace MAS.Payments.Models
{
    using System;

    public class GetPaymentsStatisticsRequest
    {
        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public long? PaymentTypeId { get; set; }
    }
}