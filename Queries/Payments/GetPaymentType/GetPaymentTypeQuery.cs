using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Queries
{
    public class GetPaymentTypeQuery : IQuery<GetPaymentTypeResponse>
    {
        public long Id { get; }

        public GetPaymentTypeQuery(long id)
        {
            Id = id;
        }
    }
}