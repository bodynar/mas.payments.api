using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Projectors;

namespace MAS.Payments.Queries
{
    internal class GetPaymentTypeQueryHandler : BaseQueryHandler<GetPaymentTypeQuery, GetPaymentTypeResponse>
    {
        private IRepository<PaymentType> Repository { get; }

        public GetPaymentTypeQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<PaymentType>();
        }

        public override GetPaymentTypeResponse Handle(GetPaymentTypeQuery query)
        {
             return Repository
                        .Get(query.Id,
                        new Projector.ToFlat<PaymentType, GetPaymentTypeResponse>());
        }
    }
}