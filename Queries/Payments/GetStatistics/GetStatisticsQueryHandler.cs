namespace MAS.Payments.Queries
{
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;
    using MAS.Payments.Infrastructure.Specification;

    internal class GetStatisticsQueryHandler : BaseQueryHandler<GetStatisticsQuery, GetStatisticsResponse>
    {
        private IRepository<Payment> PaymentRepository { get; }

        public GetStatisticsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            PaymentRepository = GetRepository<Payment>();
        }

        public override GetStatisticsResponse Handle(GetStatisticsQuery query)
        {
            Specification<Payment> filter =
                new CommonSpecification<Payment>(x =>
                    x.Date.Value.Year == query.Year
                    && x.PaymentTypeId == query.PaymentTypeId);

            var response = new GetStatisticsResponse
            {
                PaymentTypeId = query.PaymentTypeId,
                Year = query.Year,
                StatisticsData =
                    PaymentRepository
                        .Where(filter)
                        .Select(x => new GetStatisticsResponse.StatisticsDataItem
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