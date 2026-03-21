namespace MAS.Payments.Queries
{
    using System.Linq;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;

    using Microsoft.EntityFrameworkCore;

    internal class GetPaymentGroupQueryHandler : BaseQueryHandler<GetPaymentGroupQuery, GetPaymentGroupResponse>
    {
        private IRepository<PaymentGroup> Repository { get; }

        public GetPaymentGroupQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<PaymentGroup>();
        }

        public override async Task<GetPaymentGroupResponse> HandleAsync(GetPaymentGroupQuery query)
        {
            var item = await Repository
                .GetAll()
                .Where(x => x.Id == query.Id)
                .Select(x => new GetPaymentGroupResponse
                {
                    Id = x.Id,
                    PaymentDate = x.PaymentDate,
                    Month = x.Month,
                    Year = x.Year,
                    Comment = x.Comment,
                    Payments = x.Payments.Select(p => new GetPaymentGroupPaymentResponse
                    {
                        Id = p.Id,
                        Amount = p.Amount,
                        Description = p.Description,
                        PaymentTypeId = p.PaymentTypeId,
                        PaymentTypeName = p.PaymentType.Name,
                        PaymentTypeColor = p.PaymentType.Color,
                    }).ToList(),
                })
                .FirstOrDefaultAsync();

            return item;
        }
    }
}
