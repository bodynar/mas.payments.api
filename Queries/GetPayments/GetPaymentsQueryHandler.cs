using System.Collections.Generic;
using System.Linq;
using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Infrastructure.Specification;

namespace MAS.Payments.Queries
{
    internal class GetPaymentsQueryHandler : BaseQueryHandler<GetPaymentsQuery, IEnumerable<GetPaymentsResponse>>
    {
        private IRepository<Payment> Repository { get; }

        public GetPaymentsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<Payment>();
        }

        public override IEnumerable<GetPaymentsResponse> Handle(GetPaymentsQuery query)
        {
            Specification<Payment> filter = new CommonSpecification<Payment>(x => true);

            if (query.Month.HasValue)
            {
                // todo: check (sql)
                filter = filter && new CommonSpecification<Payment>(x => x.Date.HasValue && x.Date.Value.Month == query.Month.Value);
            }

            if (query.ExactAmount.HasValue)
            {
                filter = filter && new CommonSpecification<Payment>(x => x.Amount == query.ExactAmount);
            }
            else
            {
                if (query.MinAmount.HasValue)
                {
                    filter = filter && new CommonSpecification<Payment>(x => x.Amount > query.MinAmount);
                }
                if (query.MaxAmount.HasValue)
                {
                    filter = filter && new CommonSpecification<Payment>(x => x.Amount < query.MaxAmount);
                }
            }

            return Repository
                   .Where(filter)
                   .Select(x => new GetPaymentsResponse
                   {
                       Id = x.Id,
                       Amount = x.Amount,
                       Description = x.Description,
                       Date = x.Date,
                       PaymentType = x.PaymentType.Name
                   })
                   .ToList();
        }
    }
}