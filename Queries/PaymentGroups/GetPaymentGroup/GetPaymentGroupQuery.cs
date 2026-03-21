namespace MAS.Payments.Queries
{
    using MAS.Payments.Infrastructure.Query;

    public class GetPaymentGroupQuery(
        Guid id
    ) : IQuery<GetPaymentGroupResponse>
    {
        public Guid Id { get; } = id;
    }
}
