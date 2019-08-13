using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Queries
{
    public class GetPaymentQuery : IQuery<GetPaymentResponse>
    {
        public long Id { get; }

        public GetPaymentQuery(long id)
        {
            Id = id;
        }
    }
}