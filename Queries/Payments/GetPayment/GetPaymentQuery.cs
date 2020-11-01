namespace MAS.Payments.Queries
{
    using MAS.Payments.Infrastructure.Query;

    public class GetPaymentQuery : IQuery<GetPaymentResponse>
    {
        public long Id { get; }

        public GetPaymentQuery(long id)
        {
            Id = id;
        }
    }
}