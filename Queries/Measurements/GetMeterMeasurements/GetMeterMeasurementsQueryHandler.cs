namespace MAS.Payments.Queries
{
    using System.Collections.Generic;
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;
    using MAS.Payments.Infrastructure.Specification;

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
                   .Where(filter)
                   .Select(x => new GetMeterMeasurementsQueryResponse
                   {
                       Id = x.Id,
                       DateYear = x.Date.Year,
                       DateMonth = x.Date.Month,
                       Measurement = x.Measurement,
                       Comment = x.Comment,
                       IsSent = x.IsSent,
                       MeterMeasurementTypeId = x.MeterMeasurementTypeId,
                       MeasurementTypeColor = x.MeasurementType.Color,
                       MeasurementTypeName = x.MeasurementType.Name,
                   })
                   .OrderBy(x => x.DateYear)
                   .ThenBy(x => x.DateMonth)
                   .ToList();

            return filteredMeasurements;
        }
    }
}
