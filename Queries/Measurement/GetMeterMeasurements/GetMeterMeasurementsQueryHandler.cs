using System.Collections.Generic;

using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Infrastructure.Specification;
using MAS.Payments.Projectors;

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
            Specification<MeterMeasurement> filter = new BelongsToUser<MeterMeasurement>(query.UserId);

            if (query.Month.HasValue)
            {
                filter = filter && new CommonSpecification<MeterMeasurement>(x => x.Date.Month == (query.Month.Value - 1));
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
                   .Where(filter, new Projector.ToFlat<MeterMeasurement, GetMeterMeasurementsResponse>());
        }
    }
}