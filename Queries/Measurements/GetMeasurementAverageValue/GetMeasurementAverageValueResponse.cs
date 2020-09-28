namespace MAS.Payments.Queries
{
    using System.Collections.Generic;

    public class GetMeasurementAverageValueResponse
    {
        public double TotalAverageValue { get; set; }

        public IEnumerable<GetPaymentAverageValueResponseAverageYear> AverageByYears { get; set; }

        public IEnumerable<GetPaymentAverageValueResponseAverageYearAndMonth> AverageByMonthAndYears { get; set; }
    }

    public class GetMeasurementAverageValueResponseAverageYear
    {
        public double? Value { get; set; }

        public int Year { get; set; }
    }

    public class GetMeasurementAverageValueResponseAverageYearAndMonth : GetPaymentAverageValueResponseAverageYear
    {
        public int Month { get; set; }
    }
}