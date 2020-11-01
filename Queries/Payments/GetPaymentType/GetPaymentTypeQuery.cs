namespace MAS.Payments.Queries
{
    using MAS.Payments.Infrastructure.Query;

    public class GetPaymentTypeQuery : IQuery<GetPaymentTypeResponse>
    {
        public long Id { get; }

        public GetPaymentTypeQuery(long id)
        {
            Id = id;
        }
    }
}