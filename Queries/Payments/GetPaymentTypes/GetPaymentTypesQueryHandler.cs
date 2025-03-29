namespace MAS.Payments.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;

    using Microsoft.EntityFrameworkCore;

    internal class GetPaymentTypesQueryHandler : BaseQueryHandler<GetPaymentTypesQuery, IEnumerable<GetPaymentTypesResponse>>
    {
        private IRepository<PaymentType> Repository { get; }

        public GetPaymentTypesQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<PaymentType>();
        }

        public override async Task<IEnumerable<GetPaymentTypesResponse>> HandleAsync(GetPaymentTypesQuery query)
        {
            return await Repository
                   .GetAll()
                   .Select(x => new GetPaymentTypesResponse
                   {
                       Id = x.Id,
                       SystemName = x.SystemName,
                       Name = x.Name,
                       Description = x.Description,
                       Company = x.Company,
                       Color = x.Color,
                       HasRelatedPayments = x.Payments.Any(),
                       HasRelatedMeasurementTypes = x.MeasurementTypes.Any(),
                   })
                   .ToListAsync();
        }
    }
}