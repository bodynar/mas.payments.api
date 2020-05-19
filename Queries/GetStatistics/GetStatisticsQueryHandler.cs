using System;
using System.Collections.Generic;
using System.Linq;
using MAS.Payments.Comparators;
using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Infrastructure.Specification;

namespace MAS.Payments.Queries
{
    internal class GetStatisticsQueryHandler : BaseQueryHandler<GetStatisticsQuery, GetStatisticsResponse>
    {
        private IRepository<Payment> PaymentRepository { get; }

        private IRepository<MeterMeasurement> MeterMeasurementRepository { get; }

        private IEqualityComparer<DateTime> DatesComparator
            => _datesComparator.Value;

        private Lazy<IEqualityComparer<DateTime>> _datesComparator
            => new Lazy<IEqualityComparer<DateTime>>(() => new DatesComparator());

        public GetStatisticsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            PaymentRepository = GetRepository<Payment>();
            MeterMeasurementRepository = GetRepository<MeterMeasurement>();
        }

        public override GetStatisticsResponse Handle(GetStatisticsQuery query)
        {
            Specification<Payment> filter = new CommonSpecification<Payment>(x => true);

            if (query.Year.HasValue)
            {
                filter = new CommonSpecification<Payment>(x => x.Date.Value.Year == query.Year.Value);
            }
            else if (query.IsDatePeriodSpecified)
            {
                filter = new CommonSpecification<Payment>(x => x.Date <= query.To && x.Date >= query.From);
            }

            var response = new GetStatisticsResponse
            {
                Items =
                PaymentRepository
                    .Where(filter)
                    .ToList()
                    .GroupBy(x => x.PaymentType)
                    .Select(x => new StatisticsItem
                    {
                        PaymentTypeId = x.Key.Id,
                        PaymentTypeName = x.Key.Name,

                        Payments =
                            x.Key.Payments
                            .Select(y => new GetStatisticsPayment
                            {
                                Id = y.Id,
                                Amount = y.Amount,
                                Date = y.Date,

                                Measurements =
                                    query.IncludeMeasurements
                                    ? GetMeasurements(x.Key.Id, query.Year, query.From, query.To)
                                      .GroupBy(m => m.Date.Date)
                                      .Select(m => new GetStatisticsMeasurements
                                      {
                                          Date = m.Key,
                                          Measurements = m.Select(g => new GetStatisticsMeasurement
                                          {
                                              Name = g.MeasurementType.Name,
                                              Measurement = g.Measurement,
                                              SystemName = g.MeasurementType.SystemName
                                          }).ToList()
                                      })
                                      .ToList()
                                    : new List<GetStatisticsMeasurements>()
                            })
                    })
                    .ToList()
            };

            if (query.IncludeMeasurements)
            {
                PopulateMeasurementData(response);
            }

            return response;
        }

        private IQueryable<MeterMeasurement> GetMeasurements(long paymentTypeId, int? year, DateTime? from, DateTime? to)
        {
            Specification<MeterMeasurement> filter = new CommonSpecification<MeterMeasurement>(x => x.MeasurementType.PaymentTypeId == paymentTypeId);

            if (year.HasValue)
            {
                filter = filter && new CommonSpecification<MeterMeasurement>(x => x.Date.Year == year.Value);
            }
            else
            {
                if (from.HasValue)
                {
                    filter = filter && new CommonSpecification<MeterMeasurement>(x => x.Date >= from.Value);
                }
                if (to.HasValue)
                {
                    filter = filter && new CommonSpecification<MeterMeasurement>(x => x.Date <= to.Value);
                }
            }

            return MeterMeasurementRepository.Where(filter);
        }

        private void PopulateMeasurementData(GetStatisticsResponse response)
        {
            var measurementDates =
                GetAllDates(response.Items);

            response.Dates.AddRange(measurementDates);

            var dates =
                response.Items
                .SelectMany(x => x.Payments.SelectMany(y => y.Measurements));

            foreach (var statsItem in response.Items)
            {
                foreach (var paymentItem in statsItem.Payments)
                {
                    var notIntersectDates =
                        measurementDates
                        .Except(paymentItem.Measurements.Select(x => x.Date))
                        .Select(x => new GetStatisticsMeasurements
                        {
                            Date = x
                        });

                    var allDates =
                        paymentItem.Measurements
                            .Union(notIntersectDates)
                            .OrderBy(x => x.Date.Year)
                            .ThenBy(x => x.Date.Month);

                    paymentItem.Measurements.Clear();
                    paymentItem.Measurements.AddRange(allDates);
                }
            }
        }

        private IEnumerable<DateTime> GetAllDates(IEnumerable<StatisticsItem> items)
        {
            return items
                    .SelectMany(x => x.Payments.SelectMany(y => y.Measurements.Select(z => z.Date)))
                    .Distinct(DatesComparator)
                    .OrderBy(x => x.Year)
                    .ThenBy(x => x.Month)
                    ;
        }
    }
}