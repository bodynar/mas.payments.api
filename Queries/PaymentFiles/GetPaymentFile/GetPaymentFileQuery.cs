namespace MAS.Payments.Queries
{
    using MAS.Payments.Infrastructure.Query;

    public class GetPaymentFileQuery(
        long id
    ) : IQuery<GetPaymentFileResponse>
    {
        public long Id { get; } = id;
    }
}
