using System.Collections.Generic;
using System;

namespace MAS.Payments.Queries
{
    public class GetStatisticsMeasurements
    {
        public string Name { get; set; }

        public double Measurement { get; set; }
    }

    public class GetStatisticsPayment
    {
        public double Amount { get; set; }

        public DateTime? Date { get; set; }

        public ICollection<GetStatisticsMeasurements> Measurements { get; set; }

        public GetStatisticsPayment()
        {
            Measurements = new List<GetStatisticsMeasurements>();
        }
    }

    public class GetStatisticsResponse
    {
        public string PaymentTypeName { get; set; }

        public long PaymentTypeId { get; set; }

        public IEnumerable<GetStatisticsPayment> Payments { get; set; }

        public GetStatisticsResponse()
        {
            Payments = new List<GetStatisticsPayment>();
        }
    }
}