using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Query;

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
            var payment = Repository.Get(query.Id);

            // todo projector
            return new GetPaymentResponse
            {
                Id = payment.Id,
                Amount = payment.Amount,
                Date = payment.Date,
                Description = payment.Description,
                PaymentType = payment.PaymentType.Name,
                PaymentTypeId = payment.PaymentTypeId
            };
        }
    }
}