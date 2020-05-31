using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Projectors;

namespace MAS.Payments.Queries
{
    internal class GetMeterMeasurementTypeQueryHandler : BaseQueryHandler<GetMeterMeasurementTypeQuery, GetMeterMeasurementTypeResponse>
    {
        private IRepository<MeterMeasurementType> Repository { get; }
        
        public GetMeterMeasurementTypeQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<MeterMeasurementType>();
        }

        public override GetMeterMeasurementTypeResponse Handle(GetMeterMeasurementTypeQuery query)
        {
            return Repository
                   .Get(query.Id, new Projector.ToFlat<MeterMeasurementType, GetMeterMeasurementTypeResponse>());
        }
    }
}