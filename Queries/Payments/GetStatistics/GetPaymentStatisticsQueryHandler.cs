namespace MAS.Payments.Queries
{
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;
    using MAS.Payments.Infrastructure.Specification;

    internal class GetPaymentStatisticsQueryHandler : BaseQueryHandler<GetPaymentStatisticsQuery, GetPaymentStatisticsResponse>
    {
        private IRepository<Payment> PaymentRepository { get; }

        public GetPaymentStatisticsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            PaymentRepository = GetRepository<Payment>();
        }

        public override GetPaymentStatisticsResponse Handle(GetPaymentStatisticsQuery query)
        {
            Specification<Payment> filter =
                new CommonSpecification<Payment>(x =>
                    x.Date.Value.Year == query.Year
                    && x.PaymentTypeId == query.PaymentTypeId);

            var response = new GetPaymentStatisticsResponse
            {
                PaymentTypeId = query.PaymentTypeId,
                Year = query.Year,
                StatisticsData =
                    PaymentRepository
                        .Where(filter)
                        .Select(x => new GetPaymentStatisticsResponse.StatisticsDataItem
                        {
                            Month = x.Date.Value.Month,
                            Year = x.Date.Value.Year,
                            Amount = x.Amount,
                        })
                        .ToList()
            };

            return response;
        }
    }
}