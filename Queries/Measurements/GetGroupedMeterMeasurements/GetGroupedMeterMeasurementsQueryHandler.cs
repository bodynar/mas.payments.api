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

    internal class GetGroupedMeterMeasurementsQueryHandler : BaseQueryHandler<GetGroupedMeterMeasurementsQuery, IEnumerable<GetGroupedMeterMeasurementsResponse>>
    {
        private IRepository<MeterMeasurement> Repository { get; }

        public GetGroupedMeterMeasurementsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<MeterMeasurement>();
        }

        public override IEnumerable<GetGroupedMeterMeasurementsResponse> Handle(GetGroupedMeterMeasurementsQuery query)
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
                   .Where(filter)
                   .ToList();

            var result = new List<GetGroupedMeterMeasurementsResponse>();
            var itemProjector = new Projector.ToFlat<MeterMeasurement, GetGroupedMeterMeasurementsResponseMeasurement>();

            foreach (var measurement in filteredMeasurements)
            {
                var group = result.FirstOrDefault(x => x.DateMonth == measurement.Date.Month && x.DateYear == measurement.Date.Year);

                if (group != null)
                {
                    group.Measurements.Add(itemProjector.Project(measurement));
                } else
                {
                    group = new GetGroupedMeterMeasurementsResponse
                    {
                        DateMonth = measurement.Date.Month,
                        DateYear = measurement.Date.Year,
                    };
                    group.Measurements.Add(itemProjector.Project(measurement));

                    result.Add(group);
                }
            }

            result.ForEach(x => x.SortMeasurements());

            return result.OrderBy(x => x.DateYear).ThenBy(x => x.DateMonth);
        }
    }
}