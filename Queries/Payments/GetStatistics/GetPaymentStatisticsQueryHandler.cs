namespace MAS.Payments.Queries
{
    using System.Collections.Generic;
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
            var payments =
                PaymentRepository.Where(
                    new CommonSpecification<Payment>(x =>
                        x.Date.Value.Year == query.Year
                        && x.PaymentTypeId == query.PaymentTypeId)
                )
                .OrderBy(x => x.Date)
                .ToList();

            var mappedPayments =
               payments
                   .Select(x => new { Month = x.Date.Value.Month, Amount = x.Amount })
                   .ToDictionary(x => x.Month, x => x.Amount as double?);

            var response = new GetPaymentStatisticsResponse
            {
                PaymentTypeId = query.PaymentTypeId,
                Year = query.Year,
                StatisticsData =
                    Enumerable.Range(1, 12).Select(x => new GetPaymentStatisticsResponse.StatisticsDataItem
                    {
                        Month = x,
                        Year = query.Year,
                        Amount = mappedPayments.GetValueOrDefault(x, null)
                    })
            };

            return response;
        }
    }
}