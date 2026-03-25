namespace MAS.Payments.Queries
{
    using System.Linq;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;

    using Microsoft.EntityFrameworkCore;

    internal class GetPaymentGroupTemplateQueryHandler : BaseQueryHandler<GetPaymentGroupTemplateQuery, GetPaymentGroupTemplateResponse>
    {
        private IRepository<PaymentGroupTemplate> Repository { get; }

        public GetPaymentGroupTemplateQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<PaymentGroupTemplate>();
        }

        public override async Task<GetPaymentGroupTemplateResponse> HandleAsync(GetPaymentGroupTemplateQuery query)
        {
            var item = await Repository
                .GetAll()
                .Where(x => x.Id == query.Id)
                .Select(x => new GetPaymentGroupTemplateResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    PaymentTypes = x.Items.Select(i => new GetPaymentGroupTemplateItemResponse
                    {
                        PaymentTypeId = i.PaymentTypeId,
                        PaymentTypeName = i.PaymentType.Name,
                        PaymentTypeColor = i.PaymentType.Color,
                        PaymentTypeCompany = i.PaymentType.Company,
                    }).ToList(),
                })
                .FirstOrDefaultAsync();

            return item;
        }
    }
}
