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

    internal class GetMeasurementAverageValueQueryHandler : BaseQueryHandler<GetMeasurementAverageValueQuery, GetMeasurementAverageValueResponse>
    {
        private IRepository<MeterMeasurement> Repository { get; }

        private IProjector<MeterMeasurement, MeterMeasurementData> Projector { get; }

        public GetMeasurementAverageValueQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<MeterMeasurement>();

            Projector = new Projector.Common<MeterMeasurement, MeterMeasurementData>(x => new MeterMeasurementData
            {
                Measurement = x.Measurement,
                Month = x.Date.Month,
                Year = x.Date.Year
            });
        }

        public override GetMeasurementAverageValueResponse Handle(GetMeasurementAverageValueQuery query)
        {
            var payments = Repository.GetAll(Projector).ToList();
            var totalAverage = payments.Select(x => x.Measurement).Average();
            var groupedByYears = payments.GroupBy(x => x.Year);

            var averageValuesByYear =
                groupedByYears
                    .Select(group => new GetPaymentAverageValueResponseAverageYear
                    {
                        Value = Math.Round(group.Average(x => x.Measurement), 2),
                        Year = group.Key
                    });

            var yearDataItems =
                groupedByYears
                    .Select(x => x.Key)
                    .Select(year => new
                    {
                        Year = year,
                        Months = Enumerable.Range(1, 12)
                            .Select(x => groupedByYears.FirstOrDefault(y => y.Key == year)?.FirstOrDefault(y => y.Month == x) ?? new MeterMeasurementData { Year = year, Month = x, Measurement = 0 })
                    })
                    .SelectMany(group =>
                        group.Months.Select(y =>
                            new MeterMeasurementData
                            {
                                Month = y.Month,
                                Measurement = y.Measurement,
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
                        .Select(x => x.Measurement)
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

            return new GetMeasurementAverageValueResponse
            {
                TotalAverageValue = Math.Round(totalAverage, 2),
                AverageByYears = averageValuesByYear,
                AverageByMonthAndYears = averageMonthValues
            };
        }

        private class MeterMeasurementData
        {
            public double Measurement { get; set; }

            public int Year { get; set; }

            public int Month { get; set; }

            public DateTime Date { get; set; }

            public double? Average { get; set; }
        }
    }
}