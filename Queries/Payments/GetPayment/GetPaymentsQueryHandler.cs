using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Projectors;

namespace MAS.Payments.Queries
{
    internal class GetPaymentQueryHandler : BaseQueryHandler<GetPaymentQuery, GetPaymentResponse>
    {
        private IRepository<Payment> Repository { get; }

        public GetPaymentQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<Payment>();
        }

        public override GetPaymentResponse Handle(GetPaymentQuery query)
        {
            return Repository
                        .Get(query.Id,
                        new Projector.ToFlat<Payment, GetPaymentResponse>());
        }
    }
}