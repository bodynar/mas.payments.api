using System.Collections.Generic;
using System.Linq;
using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Query;

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

        public override IEnumerable<GetPaymentsResponse> Handle(GetPaymentsQuery Query)
        {
            return Repository
                   .Where(Query.Filter)
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