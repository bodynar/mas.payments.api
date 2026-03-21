namespace MAS.Payments.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;
    using MAS.Payments.Infrastructure.Specification;

    using Microsoft.EntityFrameworkCore;

    internal class GetPaymentGroupsQueryHandler : BaseQueryHandler<GetPaymentGroupsQuery, IEnumerable<GetPaymentGroupsResponse>>
    {
        private IRepository<PaymentGroup> Repository { get; }

        public GetPaymentGroupsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<PaymentGroup>();
        }

        public override async Task<IEnumerable<GetPaymentGroupsResponse>> HandleAsync(GetPaymentGroupsQuery query)
        {
            Specification<PaymentGroup> filter = new CommonSpecification<PaymentGroup>(x => true);

            if (query.Month.HasValue)
            {
                filter = filter && new CommonSpecification<PaymentGroup>(x => x.Month == query.Month.Value);
            }

            if (query.Year.HasValue)
            {
                filter = filter && new CommonSpecification<PaymentGroup>(x => x.Year == query.Year.Value);
            }

            return await Repository
                .Where(filter)
                .Select(x => new GetPaymentGroupsResponse
                {
                    Id = x.Id,
                    PaymentDate = x.PaymentDate,
                    Month = x.Month,
                    Year = x.Year,
                    Comment = x.Comment,
                    PaymentsCount = x.Payments.Count,
                    TotalAmount = x.Payments.Sum(p => p.Amount),
                })
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ToListAsync();
        }
    }
}
