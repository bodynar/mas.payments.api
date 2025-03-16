namespace MAS.Payments.Queries
{
    using MAS.Payments.Infrastructure.Query;

    public class GetPaymentTypeQuery(
        long id
    ) : IQuery<GetPaymentTypeResponse>
    {
        public long Id { get; } = id;
    }
}