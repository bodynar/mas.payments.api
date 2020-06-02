namespace MAS.Payments.Models
{
    public class GetStatisticsRequest
    {
        public short? Year { get; set;  }

        public long? PaymentTypeId { get; set; }
    }
}