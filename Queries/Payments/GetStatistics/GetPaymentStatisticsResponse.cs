namespace MAS.Payments.Queries
{
    using System.Collections.Generic;

    public class GetPaymentStatisticsResponse
    {
        public short Year { get; set; }

        public IEnumerable<TypeStatisticsItem> TypeStatistics { get; set; }

        public class TypeStatisticsItem
        {
            public long PaymentTypeId { get; set; }

            public string PaymentTypeName { get; set; }

            public IEnumerable<StatisticsDataItem> StatisticsData { get; set; }

            public class StatisticsDataItem
            {
                public int Month { get; set; }

                public int Year { get; set; }

                public double? Amount { get; set; }
            }
        }
    }
}