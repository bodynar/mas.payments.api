namespace MAS.Payments.Queries
{
    using MAS.Payments.Infrastructure.Query;

    public class GetPaymentTypeQuery(
        Guid id
    ) : IQuery<GetPaymentTypeResponse>
    {
        public Guid Id { get; } = id;
    }
}
