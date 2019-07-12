using System.Collections.Generic;
using System.Linq;
using MAS.Payments.DataBase;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Extensions;
using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Queries
{
    internal class GetPaymentsQueryHandler : BaseQueryHandler<GetPaymentsQuery, IEnumerable<GetPaymentsQueryResponse>>
    {
        public GetPaymentsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
        }

        public override IEnumerable<GetPaymentsQueryResponse> Handle(GetPaymentsQuery Query)
        {
            return GetRepository<Payment>()
                   .GetAll()
                   .Where(Query.Filter)
                   .Select(x => new GetPaymentsQueryResponse
                   {
                       Id = x.Id,
                       Amount = x.Amount,
                       Description = x.Description,
                       Date = x.Date,
                       PaymentType = x.PaymentType.Name,
                       PaymentTypeId = x.PaymentType.Id
                   })
                   .ToList();
        }
    }
}