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

    internal class GetMeasurementStatisticsQueryHandler : BaseQueryHandler<GetMeasurementStatisticsQuery, GetMeasurementStatisticsQueryResponse>
    {
        private static readonly IEnumerable<int> MonthNumbers =
            Enumerable.Range(1, 12);

        private IRepository<MeterMeasurement> MeasurementRepository { get; }

        public GetMeasurementStatisticsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            MeasurementRepository = GetRepository<MeterMeasurement>();
        }

        public override GetMeasurementStatisticsQueryResponse Handle(GetMeasurementStatisticsQuery query)
        {
            if (query.From.HasValue && query.To.HasValue && query.From.Value >= query.To.Value)
            {
                throw new ArgumentException($"{nameof(query.From)} value must be less than {nameof(query.To)}.");
            }

            Specification<MeterMeasurement> filter = new CommonSpecification<MeterMeasurement>(x => true);

            if (query.MeasurementTypeId.HasValue)
            {
                filter &= new CommonSpecification<MeterMeasurement>(x => x.MeterMeasurementTypeId == query.MeasurementTypeId);
            }
            if (query.From.HasValue)
            {
                filter &= new CommonSpecification<MeterMeasurement>(x => x.Date.Date >= query.From.Value.Date);
            }
            if (query.To.HasValue)
            {
                filter &= new CommonSpecification<MeterMeasurement>(x => x.Date.Date <= query.To.Value.Date);
            }

            var measurements =
                MeasurementRepository.Where(filter)
                .ToList()
                .GroupBy(x => x.MeterMeasurementTypeId);

            var response = new GetMeasurementStatisticsQueryResponse()
            {
                From = query.From,
                To = query.To
            };

            foreach (var typeGroup in measurements)
            {
                var firstItem = typeGroup.FirstOrDefault();

                if (firstItem == null)
                {
                    continue;
                }

                var typeStatisticsItem = new MeasurementTypeStatisticsItem()
                {
                    MeasurementTypeId = typeGroup.Key,
                    MeasurementTypeName = firstItem.MeasurementType.Name
                };

                var groupedByYear = typeGroup.GroupBy(x => x.Date.Year).OrderBy(x => x.Key);

                foreach (var yearGroup in groupedByYear)
                {
                    var months = MonthNumbers;

                    if (query.From.HasValue && query.From.Value.Year == yearGroup.Key)
                    {
                        if (query.From.Value.Month > 1)
                        {
                            months = months.Where(x => x >= query.From.Value.Month);
                        }
                    }
                    else
                    {
                        var isFirstYear = groupedByYear.First().Key == yearGroup.Key;

                        if (isFirstYear)
                        {
                            var month = yearGroup.Select(x => x.Date.Month).Min();

                            if (month > 0 && month < 12)
                            {
                                months = months.Where(x => x >= month);
                            }
                        }
                    }

                    if (query.To.HasValue && query.To.Value.Year == yearGroup.Key)
                    {
                        if (query.To.Value.Month < 12)
                        {
                            months = months.Where(x => x <= query.To.Value.Month);
                        }
                    }
                    else
                    {
                        var isLastYear = groupedByYear.Last().Key == yearGroup.Key;

                        if (isLastYear)
                        {
                            var month = yearGroup.Select(x => x.Date.Month).Max();

                            if (month > 0 && month < 12)
                            {
                                months = months.Where(x => x <= month);
                            }
                        }
                    }
                    foreach (var monthNumber in months)
                    {
                        var item = yearGroup.FirstOrDefault(x => x.Date.Month == monthNumber);

                        typeStatisticsItem.StatisticsData.Add(new MeasurementStatisticsDataItem
                        {
                            Year = yearGroup.Key,
                            Month = monthNumber,
                            MeasurementDiff = item?.Diff
                        });
                    }
                }

                response.TypeStatistics.Add(typeStatisticsItem);
            }

            return response;
        }
    }
}
