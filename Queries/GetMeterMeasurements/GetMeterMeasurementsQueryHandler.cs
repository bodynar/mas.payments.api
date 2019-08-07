using System.Collections.Generic;
using System.Linq;
using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Infrastructure.Specification;

namespace MAS.Payments.Queries
{
    internal class GetMeterMeasurementsQueryHandler : BaseQueryHandler<GetMeterMeasurementsQuery, IEnumerable<GetMeterMeasurementsResponse>>
    {
        private IRepository<MeterMeasurement> Repository { get; }

        public GetMeterMeasurementsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<MeterMeasurement>();
        }

        public override IEnumerable<GetMeterMeasurementsResponse> Handle(GetMeterMeasurementsQuery query)
        {
            Specification<MeterMeasurement> filter = new CommonSpecification<MeterMeasurement>(x => true);

            if (query.Month.HasValue)
            {
                // todo: check (sql)
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

            return Repository
                   .Where(filter)
                   .Select(x => new GetMeterMeasurementsResponse
                   {
                       Id = x.Id,
                       Measurement = x.Measurement,
                       Comment = x.Comment,
                       Date = x.Date,
                       MeterMeasurementTypeId = x.MeterMeasurementTypeId,
                       MeasurementTypeName = x.MeasurementType.Name
                   })
                   .ToList();
        }
    }
}