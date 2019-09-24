using System.Collections.Generic;
using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Projectors;

namespace MAS.Payments.Queries
{
    internal class GetMeterMeasurementTypesQueryHandler : BaseQueryHandler<GetMeterMeasurementTypesQuery, IEnumerable<GetMeterMeasurementTypesResponse>>
    {
        private IRepository<MeterMeasurementType> Repository { get; }
        
        public GetMeterMeasurementTypesQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<MeterMeasurementType>();
        }

        public override IEnumerable<GetMeterMeasurementTypesResponse> Handle(GetMeterMeasurementTypesQuery query)
        {
            return Repository
                   .GetAll(new Projector.ToFlat<MeterMeasurementType, GetMeterMeasurementTypesResponse>());
        }
    }
}