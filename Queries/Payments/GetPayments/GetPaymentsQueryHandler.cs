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

    internal class GetPaymentsQueryHandler : BaseQueryHandler<GetPaymentsQuery, IEnumerable<GetPaymentsResponse>>
    {
        private IRepository<Payment> Repository { get; }

        public GetPaymentsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<Payment>();
        }

        public override async Task<IEnumerable<GetPaymentsResponse>> HandleAsync(GetPaymentsQuery query)
        {
            Specification<Payment> filter = new CommonSpecification<Payment>(x => true);

            if (query.PaymentTypeId.HasValue)
            {
                filter = filter && new CommonSpecification<Payment>(x => x.PaymentTypeId == query.PaymentTypeId);
            }

            if (query.Month.HasValue)
            {
                filter = filter && new CommonSpecification<Payment>(x => x.Date.HasValue && x.Date.Value.Month == query.Month.Value);
            }

            if (query.Year.HasValue)
            {
                filter = filter && new CommonSpecification<Payment>(x => x.Date.HasValue && x.Date.Value.Year == query.Year.Value);
            }

            if (query.ExactAmount.HasValue)
            {
                filter = filter && new CommonSpecification<Payment>(x => x.Amount == query.ExactAmount);
            }
            else
            {
                if (query.MinAmount.HasValue)
                {
                    filter = filter && new CommonSpecification<Payment>(x => x.Amount >= query.MinAmount.Value);
                }

                if (query.MaxAmount.HasValue)
                {
                    filter = filter && new CommonSpecification<Payment>(x => x.Amount <= query.MaxAmount.Value);
                }
            }

            return await Repository
                .Where(filter)
                .Select(x =>
                    new GetPaymentsResponse
                    {
                        Id = x.Id,
                        Amount = x.Amount,
                        DateYear = x.Date.HasValue ? x.Date.Value.Year : 0,
                        DateMonth = x.Date.HasValue ? x.Date.Value.Month : 0,
                        Description = x.Description,
                        PaymentTypeName = x.PaymentType.Name,
                        PaymentTypeColor = x.PaymentType.Color,
                        PaymentTypeId = x.PaymentTypeId,
                        ReceiptFileName = x.ReceiptId != default ? x.Receipt.Name : null,
                        CheckFileName = x.CheckId != default ? x.Check.Name : null,
                    })
                .ToListAsync();
        }
    }
}