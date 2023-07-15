namespace MAS.Payments.Queries
{
    using System.Collections.Generic;
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;
    using MAS.Payments.Infrastructure.Specification;
    using MAS.Payments.Projectors;

    internal class GetMeterMeasurementsQueryHandler : BaseQueryHandler<GetMeterMeasurementsQuery, IEnumerable<GetMeterMeasurementsQueryResponse>>
    {
        private IRepository<MeterMeasurement> Repository { get; }

        public GetMeterMeasurementsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<MeterMeasurement>();
        }

        public override IEnumerable<GetMeterMeasurementsQueryResponse> Handle(GetMeterMeasurementsQuery query)
        {
            Specification<MeterMeasurement> filter = new CommonSpecification<MeterMeasurement>(x => true);

            if (query.Month.HasValue)
            {
                filter = filter && new CommonSpecification<MeterMeasurement>(x => x.Date.Month == query.Month.Value);
            }

            if (query.Year.HasValue)
            {
                filter = filter && new CommonSpecification<MeterMeasurement>(x => x.Date.Year == query.Year.Value);
            }

            if (query.MeterMeasurementTypeId.HasValue)
            {
                filter = filter && new CommonSpecification<MeterMeasurement>(x => x.MeterMeasurementTypeId == query.MeterMeasurementTypeId.Value);
            }

            var filteredMeasurements =
                Repository
                   .Where(filter, new Projector.ToFlat<MeterMeasurement, GetMeterMeasurementsQueryResponse>())
                   .ToList()
                   .OrderBy(x => x.DateYear)
                   .ThenBy(x => x.DateMonth)
                   .ToList();

            return filteredMeasurements;
        }
    }
}
