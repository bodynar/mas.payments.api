namespace MAS.Payments.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;
    using MAS.Payments.Infrastructure.Specification;

    internal class GetPaymentStatisticsQueryHandler : BaseQueryHandler<GetPaymentStatisticsQuery, GetPaymentStatisticsResponse>
    {
        private static readonly IEnumerable<int> MonthNumbers =
            Enumerable.Range(1, 12);

        private IRepository<Payment> PaymentRepository { get; }

        public GetPaymentStatisticsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            PaymentRepository = GetRepository<Payment>();
        }

        public override GetPaymentStatisticsResponse Handle(GetPaymentStatisticsQuery query)
        {
            if (query.From.HasValue && query.To.HasValue && query.From.Value >= query.To.Value)
            {
                throw new ArgumentException($"{nameof(query.From)} value must be less than {nameof(query.To)}.");
            }

            Specification<Payment> filter = new CommonSpecification<Payment>(x => true);

            if (query.PaymentTypeId.HasValue)
            {
                filter &= new CommonSpecification<Payment>(x => x.PaymentTypeId == query.PaymentTypeId);
            }
            if (query.From.HasValue)
            {
                filter &= new CommonSpecification<Payment>(x => x.Date.Value.Date >= query.From.Value.Date);
            }
            if (query.To.HasValue)
            {
                filter &= new CommonSpecification<Payment>(x => x.Date.Value.Date <= query.To.Value.Date);
            }

            var payments =
                PaymentRepository.Where(filter)
                .OrderBy(x => x.Date)
                .Select(x => new
                {
                    Date = x.Date.Value,
                    x.Amount,
                    x.PaymentTypeId,
                    PaymentTypeName = x.PaymentType.Name
                })
                .ToList()
                .GroupBy(x => x.PaymentTypeId);

            var response = new GetPaymentStatisticsResponse
            {
                From = query.From,
                To = query.To
            };

            foreach (var paymentTypeGroup in payments)
            {
                var firstItem = paymentTypeGroup.FirstOrDefault();

                if (firstItem == null)
                {
                    continue;
                }

                var typeStatisticsItem = new PaymentTypeStatisticsItem()
                {
                    PaymentTypeId = paymentTypeGroup.Key,
                    PaymentTypeName = firstItem.PaymentTypeName
                };

                var groupedByYear = paymentTypeGroup.GroupBy(x => x.Date.Year).OrderBy(x => x.Key);

                foreach (var yearGroup in groupedByYear)
                {
                    foreach (var monthNumber in MonthNumbers)
                    {
                        var item = yearGroup.FirstOrDefault(x => x.Date.Month == monthNumber);

                        typeStatisticsItem.StatisticsData.Add(new PaymentStatisticsDataItem
                        {
                            Year = yearGroup.Key,
                            Month = monthNumber,
                            Amount = item?.Amount
                        });
                    }
                }

                response.TypeStatistics.Add(typeStatisticsItem);
            }


            return response;
        }
    }
}