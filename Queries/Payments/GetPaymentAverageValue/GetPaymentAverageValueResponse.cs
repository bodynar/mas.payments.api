namespace MAS.Payments.Queries
{
    using System.Collections.Generic;

    public class GetPaymentAverageValueResponse
    {
        public double TotalAverageValue { get; set; }

        public IEnumerable<GetPaymentAverageValueResponseAverageYear> AverageByYears { get; set; }

        public IEnumerable<GetPaymentAverageValueResponseAverageYearAndMonth> AverageByMonthAndYears { get; set; }
    }

    public class GetPaymentAverageValueResponseAverageYear
    {
        public double? Value { get; set; }

        public int Year { get; set; }
    }

    public class GetPaymentAverageValueResponseAverageYearAndMonth: GetPaymentAverageValueResponseAverageYear
    {
        public int Month { get; set; }
    }
}