namespace MAS.Payments.Queries
{
    using MAS.Payments.Infrastructure.Query;

    public class GetPaymentQuery(
        Guid id
    ) : IQuery<GetPaymentResponse>
    {
        public Guid Id { get; } = id;
    }
}
