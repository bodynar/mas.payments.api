namespace MAS.Payments.Queries
{
    using System.Collections.Generic;
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Projector;
    using MAS.Payments.Infrastructure.Query;
    using MAS.Payments.Projectors;

    internal class GetMeterMeasurementTypesQueryHandler : BaseQueryHandler<GetMeterMeasurementTypesQuery, IEnumerable<GetMeterMeasurementTypesResponse>>
    {
        private IRepository<MeterMeasurementType> Repository { get; }

        private IProjector<MeterMeasurementType, GetMeterMeasurementTypesResponse> Projector { get; }

        public GetMeterMeasurementTypesQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<MeterMeasurementType>();
            Projector = new Projector.ToFlat<MeterMeasurementType, GetMeterMeasurementTypesResponse>();
        }

        public override IEnumerable<GetMeterMeasurementTypesResponse> Handle(GetMeterMeasurementTypesQuery query)
        {
            return Repository
                   .GetAll()
                   .ToList()
                   .Select(item => {
                       var mapped = Projector.Project(item);
                       mapped.HasRelatedMeasurements = item.MeterMeasurements.Any();

                       return mapped;
                   }); ;
        }
    }
}