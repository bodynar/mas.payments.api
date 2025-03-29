namespace MAS.Payments.Queries
{
    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;

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
            var item = Repository.Get(query.Id);

            return new GetPaymentTypeResponse
            {
                Id = item.Id,
                SystemName = item.SystemName,
                Name = item.Name,
                Description = item.Description,
                Company = item.Company,
                Color = item.Color,
            };
        }
    }
}