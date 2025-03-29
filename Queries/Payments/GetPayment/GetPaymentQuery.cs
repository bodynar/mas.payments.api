namespace MAS.Payments.Queries
{
    using MAS.Payments.Infrastructure.Query;

    public class GetPaymentQuery(
        long id
    ) : IQuery<GetPaymentResponse>
    {
        public long Id { get; } = id;
    }
}