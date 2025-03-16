namespace MAS.Payments.Queries
{
    using System.Collections.Generic;
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;

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
                   .GetAll()
                   .Select(x => new GetMeterMeasurementTypesResponse
                   {
                       Id = x.Id,
                       SystemName = x.SystemName,
                       Name = x.Name,
                       Description = x.Description,
                       PaymentTypeId = x.PaymentTypeId,
                       PaymentTypeName = x.PaymentType.Name,
                       PaymentTypeColor = x.PaymentType.Color,
                       Color = x.Color,
                       HasRelatedMeasurements = x.MeterMeasurements.Any(),
                   })
                   .ToList();
        }
    }
}