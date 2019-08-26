using System.Linq;
using System.Collections.Generic;
using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Infrastructure.Specification;
using MAS.Payments.Projectors;

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

            if (query.PaymentTypeId.HasValue)
            {
                filter = filter && new CommonSpecification<Payment>(x => x.PaymentTypeId == query.PaymentTypeId);
            }

            if (query.Month.HasValue)
            {
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
                    filter = filter && new CommonSpecification<Payment>(x => x.Amount >= query.MinAmount.Value);
                }
                if (query.MaxAmount.HasValue)
                {
                    filter = filter && new CommonSpecification<Payment>(x => x.Amount <= query.MaxAmount.Value);
                }
            }

            return Repository
                   .Where(filter, new Projector.ToFlat<Payment, GetPaymentsResponse>()); ;
        }
    }
}