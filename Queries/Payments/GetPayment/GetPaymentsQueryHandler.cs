namespace MAS.Payments.Queries
{
    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;

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
            var item = Repository.Get(query.Id);

            return new GetPaymentResponse
            {
                Id = item.Id,
                Amount = item.Amount,
                DateYear = item.Date.HasValue ? item.Date.Value.Year : 0,
                DateMonth = item.Date.HasValue ? item.Date.Value.Month : 0,
                Description = item.Description,
                PaymentType = item.PaymentType.Name,
                PaymentTypeId = item.PaymentTypeId,
            };
        }
    }
}