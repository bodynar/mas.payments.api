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

    internal class GetPaymentGroupTemplatesQueryHandler : BaseQueryHandler<GetPaymentGroupTemplatesQuery, IEnumerable<GetPaymentGroupTemplatesResponse>>
    {
        private IRepository<PaymentGroupTemplate> Repository { get; }

        public GetPaymentGroupTemplatesQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<PaymentGroupTemplate>();
        }

        public override async Task<IEnumerable<GetPaymentGroupTemplatesResponse>> HandleAsync(GetPaymentGroupTemplatesQuery query)
        {
            return await Repository
                .GetAll()
                .Select(x => new GetPaymentGroupTemplatesResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    PaymentTypesCount = x.Items.Count,
                    PaymentTypes = x.Items.Select(i => new GetPaymentGroupTemplateItemResponse
                    {
                        PaymentTypeId = i.PaymentTypeId,
                        PaymentTypeName = i.PaymentType.Name,
                        PaymentTypeColor = i.PaymentType.Color,
                        PaymentTypeCompany = i.PaymentType.Company,
                    }).ToList(),
                })
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    }
}
