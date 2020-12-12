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
            Specification<Payment> paymentSpecification = new CommonSpecification<Payment>(x => x.Date.Value.Year == query.Year);

            if (query.PaymentTypeId.HasValue)
            {
                paymentSpecification &= new CommonSpecification<Payment>(x => x.PaymentTypeId == query.PaymentTypeId.Value);
            }

            var payments =
                PaymentRepository.Where(paymentSpecification)
                .OrderBy(x => x.Date)
                .ToList();

            var mappedPayments =
               payments
                   .Select(x => new
                   {
                       x.Date.Value.Month,
                       x.Amount,
                       x.PaymentTypeId,
                       PaymentTypeName = x.PaymentType.Name
                   })
                   .GroupBy(x => x.PaymentTypeId);

            var response = new GetPaymentStatisticsResponse
            {
                Year = query.Year,
                TypeStatistics = mappedPayments.Select(x => new GetPaymentStatisticsResponse.TypeStatisticsItem
                {
                    PaymentTypeId = x.Key,
                    PaymentTypeName = x.First().PaymentTypeName,
                    StatisticsData = Enumerable.Range(1, 12).Select(y => new GetPaymentStatisticsResponse.TypeStatisticsItem.StatisticsDataItem
                    {
                        Month = y,
                        Year = query.Year,
                        Amount = x.FirstOrDefault(z => z.Month == y)?.Amount
                    })
                })
            };

            return response;
        }
    }
}