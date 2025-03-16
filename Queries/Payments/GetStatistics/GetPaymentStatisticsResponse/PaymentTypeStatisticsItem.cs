namespace MAS.Payments.Queries
{
    using System.Collections.Generic;

    public class PaymentTypeStatisticsItem
    {
        public long PaymentTypeId { get; set; }

        public string PaymentTypeName { get; set; }

        public ICollection<PaymentStatisticsDataItem> StatisticsData { get; } = [];
    }
}
