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
        private IRepository<Payment> Repository { get; }

        public GetStatisticsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<Payment>();
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

            return Repository
                    .Where(filter)
                    .Select(payment => new GetStatisticsResponse
                    {
                        Amout = payment.Amount,
                        Date = payment.Date,
                        PaymentType = payment.PaymentType.Name,
                        Measurements =
                            !query.IncludeMeasurements
                                ? null
                                : payment.PaymentType
                                    .MeasurementTypes
                                    .SelectMany(x => x.MeterMeasurements)
                                    .Select(x => new GetStatisticsMeasurements
                                    {
                                        Name = x.MeasurementType.Name,
                                        Measurement = x.Measurement
                                    })
                    });
        }
    }
}