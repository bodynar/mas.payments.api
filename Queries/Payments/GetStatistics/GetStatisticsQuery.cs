namespace MAS.Payments.Queries
{
    using MAS.Payments.Infrastructure.Query;

    public class GetStatisticsQuery : IQuery<GetStatisticsResponse>
    {
        public short Year { get; }

        public long PaymentTypeId { get; }

        public GetStatisticsQuery(short year, long paymentTypeId)
        {
            Year = year;
            PaymentTypeId = paymentTypeId;
        }
    }
}