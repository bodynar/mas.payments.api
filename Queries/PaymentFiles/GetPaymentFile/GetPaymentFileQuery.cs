namespace MAS.Payments.Queries
{
    using MAS.Payments.Infrastructure.Query;

    public class GetPaymentFileQuery(
        Guid id
    ) : IQuery<GetPaymentFileResponse>
    {
        public Guid Id { get; } = id;
    }
}
