using System.Collections.Generic;
using System.Linq;
using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Query;

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

        public override IEnumerable<GetMeterMeasurementsResponse> Handle(GetMeterMeasurementsQuery Query)
        {
            return Repository
                   .GetAll()
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