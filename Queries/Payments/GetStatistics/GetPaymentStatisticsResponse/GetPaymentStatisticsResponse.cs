namespace MAS.Payments.Queries
{
    using System;
    using System.Collections.Generic;

    public class GetPaymentStatisticsResponse
    {
        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public ICollection<PaymentTypeStatisticsItem> TypeStatistics { get; } = [];
    }
}