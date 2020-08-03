namespace MAS.Payments.Queries
{
    using MAS.Payments.Infrastructure.Query;

    public class GetPaymentStatisticsQuery : IQuery<GetPaymentStatisticsResponse>
    {
        public short Year { get; }

        public long PaymentTypeId { get; }

        public GetPaymentStatisticsQuery(short year, long paymentTypeId)
        {
            Year = year;
            PaymentTypeId = paymentTypeId;
        }
    }
}