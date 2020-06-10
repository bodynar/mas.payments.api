namespace MAS.Payments.Models
{
    public class GetPaymentsStatisticsRequest
    {
        public short? Year { get; set;  }

        public long? PaymentTypeId { get; set; }
    }
}