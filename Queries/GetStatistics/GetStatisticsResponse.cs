using System.Collections.Generic;
using System;

namespace MAS.Payments.Queries
{
    public class GetStatisticsMeasurements
    {
        public string Name { get; set; }

        public double Measurement { get; set; }

        public DateTime Date { get; set; }
    }

    public class GetStatisticsPayment
    {
        public long Id { get; set; }

        public double Amount { get; set; }

        public DateTime? Date { get; set; }

        public List<GetStatisticsMeasurements> Measurements { get; set; }

        public GetStatisticsPayment()
        {
            Measurements = new List<GetStatisticsMeasurements>();
        }
    }

    public class StatisticsItem
    {
        public string PaymentTypeName { get; set; }

        public long PaymentTypeId { get; set; }

        public IEnumerable<GetStatisticsPayment> Payments { get; set; }
    }

    public class GetStatisticsResponse
    {
        public List<DateTime> Dates { get; set; }

        public List<StatisticsItem> Items { get; set; }

        public GetStatisticsResponse()
        {
            Dates = new List<DateTime>();
        }
    }
}