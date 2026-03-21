namespace MAS.Payments.Queries
{
    using MAS.Payments.Infrastructure.Query;

    public class GetPaymentGroupQuery(
        long id
    ) : IQuery<GetPaymentGroupResponse>
    {
        public long Id { get; } = id;
    }
}
