using System;
using System.Collections.Generic;
using System.Linq;
using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Infrastructure.Specification;

namespace MAS.Payments.Queries
{
    internal class GetStatisticsQueryHandler : BaseQueryHandler<GetStatisticsQuery, IEnumerable<GetStatisticsResponse>>
    {
        private IRepository<Payment> PaymentRepository { get; }

        private IRepository<MeterMeasurement> MeterMeasurementRepository { get; }

        public GetStatisticsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            PaymentRepository = GetRepository<Payment>();
            MeterMeasurementRepository = GetRepository<MeterMeasurement>();
        }

        public override IEnumerable<GetStatisticsResponse> Handle(GetStatisticsQuery query)
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
            // todo test measurements
            // todo group by paymenttype
            var response = PaymentRepository
                    .Where(filter)
                    .Select(x => x.PaymentType)
                    .Select(x => new GetStatisticsResponse
                    {
                        PaymentTypeId = x.Id,
                        PaymentTypeName = x.Name,

                        Payments =
                            x.Payments
                            .Select(y => new GetStatisticsPayment
                            {
                                Amount = y.Amount,
                                Date = y.Date
                            })
                    })
                    .ToList();

            if (query.IncludeMeasurements)
            {
                foreach (var paymentTypeItem in response)
                {
                    foreach (var paymentItem in paymentTypeItem.Payments)
                    {
                        var measurements =
                            GetMeasurements(paymentTypeItem.PaymentTypeId, query.Year, query.From, query.To)
                            .Select(x => new GetStatisticsMeasurements
                            {
                                Name = x.MeasurementType.Name,
                                Measurement = x.Measurement,
                            });

                        paymentItem.Measurements = new List<GetStatisticsMeasurements>(measurements);
                    }
                }
            }

            return response;
        }

        private IEnumerable<MeterMeasurement> GetMeasurements(long paymentTypeId, int? year, DateTime? from, DateTime? to)
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

            return MeterMeasurementRepository.Where(filter).ToList();
        }
    }
}