using System.Collections.Generic;
using System.Linq;
using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Queries
{
    internal class GetPaymentTypesQueryHandler : BaseQueryHandler<GetPaymentTypesQuery, IEnumerable<GetPaymentTypesResponse>>
    {
        private IRepository<PaymentType> Repository { get; }
        
        public GetPaymentTypesQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<PaymentType>();
        }

        public override IEnumerable<GetPaymentTypesResponse> Handle(GetPaymentTypesQuery Query)
        {
            return Repository
                   .GetAll()
                   .Select(x => new GetPaymentTypesResponse
                   {
                       Id = x.Id,
                       Name = x.Name,
                       Description = x.Description,
                       Company = x.Company
                   })
                   .ToList();
        }
    }
}