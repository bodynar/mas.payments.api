using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Projectors;

namespace MAS.Payments.Queries
{
    internal class GetMeterMeasurementQueryHandler : BaseQueryHandler<GetMeasurementQuery, GetMeterMeasurementResponse>
    {
        private IRepository<MeterMeasurement> Repository { get; }
        
        public GetMeterMeasurementQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<MeterMeasurement>();
        }

        public override GetMeterMeasurementResponse Handle(GetMeasurementQuery query)
        {
            return Repository
                   .Get(query.Id, new Projector.ToFlat<MeterMeasurement, GetMeterMeasurementResponse>());
        }
    }
}