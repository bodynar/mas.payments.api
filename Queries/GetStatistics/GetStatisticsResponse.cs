using System.Collections.Generic;
using System;

namespace MAS.Payments.Queries
{
    public class GetStatisticsMeasurements
    {
        public string Name { get; set; }

        public double Measurement { get; set; }
    }

    public class GetStatisticsResponse
    {
        public double Amout { get; set; }

        public DateTime? Date { get; set; }

        public string PaymentType { get; set; }

        public IEnumerable<GetStatisticsMeasurements> Measurements { get; set; }
    }
}