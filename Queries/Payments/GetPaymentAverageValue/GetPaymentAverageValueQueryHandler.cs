namespace MAS.Payments.Queries
{
    using System;
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Projector;
    using MAS.Payments.Infrastructure.Query;
    using MAS.Payments.Projectors;

    internal class GetPaymentAverageValueQueryHandler : BaseQueryHandler<GetPaymentAverageValueQuery, GetPaymentAverageValueResponse>
    {
        private IRepository<Payment> Repository { get; }

        private IProjector<Payment, PaymentData> Projector { get; }

        public GetPaymentAverageValueQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<Payment>();

            Projector = new Projector.Common<Payment, PaymentData>(x => new PaymentData
            {
                Amount = x.Amount,
                Month = x.Date.Value.Month,
                Year = x.Date.Value.Year
            });
        }

        public override GetPaymentAverageValueResponse Handle(GetPaymentAverageValueQuery query)
        {
            var payments = Repository.GetAll(Projector).ToList();
            var totalAverage = payments.Select(x => x.Amount).Average();
            var groupedByYears = payments.GroupBy(x => x.Year);

            var averageValuesByYear =
                groupedByYears
                    .Select(group => new GetPaymentAverageValueResponseAverageYear
                    {
                        Value = Math.Round(group.Average(x => x.Amount), 2),
                        Year = group.Key
                    });

            var yearDataItems =
                groupedByYears
                    .Select(x => x.Key)
                    .Select(year => new
                    {
                        Year = year,
                        Months = Enumerable.Range(1, 12)
                            .Select(x => groupedByYears.FirstOrDefault(y => y.Key == year)?.FirstOrDefault(y => y.Month == x) ?? new PaymentData { Year = year, Month = x, Amount = 0 })
                    })
                    .SelectMany(group =>
                        group.Months.Select(y =>
                            new PaymentData
                            {
                                Month = y.Month,
                                Amount = y.Amount,
                                Year = y.Year,
                                Date = new DateTime(y.Year, y.Month, 1),
                            })
                    )
                    .ToArray();

            for (int i = 0; i < yearDataItems.Length; i++)
            {
                var currentDataItem = yearDataItems[i];
                var previousDataItemsAverageValue = 
                    yearDataItems.Where(x => x.Date < currentDataItem.Date)
                        .OrderByDescending(x => x.Date)
                        .Take(3)
                        .Select(x => x.Amount)
                        .DefaultIfEmpty()
                        .Average() as double?;

                if (previousDataItemsAverageValue.HasValue)
                {
                    previousDataItemsAverageValue = Math.Round(previousDataItemsAverageValue.Value, 2);
                }

                currentDataItem.Average = previousDataItemsAverageValue;
            }

            var averageMonthValues =
                yearDataItems
                    .Select(x => new GetPaymentAverageValueResponseAverageYearAndMonth
                    {
                        Month = x.Month,
                        Year = x.Year,
                        Value = x.Average == 0 ? null : x.Average
                    });

            return new GetPaymentAverageValueResponse
            {
                TotalAverageValue = Math.Round(totalAverage, 2),
                AverageByYears = averageValuesByYear,
                AverageByMonthAndYears = averageMonthValues
            };
        }

        private class PaymentData
        {
            public double Amount { get; set; }

            public int Year { get; set; }

            public int Month { get; set; }

            public DateTime Date { get; set; }

            public double? Average { get; set; }
        }
    }
}