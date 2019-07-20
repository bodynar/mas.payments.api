using System.Collections.Generic;
using System.Linq;
using MAS.Payments.DataBase;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Queries
{
    internal class GetMeterMeasurementTypesQueryHandler : BaseQueryHandler<GetMeterMeasurementTypesQuery, IEnumerable<GetMeterMeasurementTypesResponse>>
    {
        public GetMeterMeasurementTypesQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
        }

        public override IEnumerable<GetMeterMeasurementTypesResponse> Handle(GetMeterMeasurementTypesQuery Query)
        {
            return GetRepository<MeterMeasurementType>()
                   .GetAll()
                   .Select(x => new GetMeterMeasurementTypesResponse
                   {
                       Id = x.Id,
                       Name = x.Name,
                       Description = x.Description,
                       PaymentTypeId = x.PaymentTypeId,
                       PaymentTypeName = x.PaymentType.Name
                   })
                   .ToList();
        }
    }
}