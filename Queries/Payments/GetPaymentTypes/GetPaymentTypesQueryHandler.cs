using System.Collections.Generic;
using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Projectors;

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

        public override IEnumerable<GetPaymentTypesResponse> Handle(GetPaymentTypesQuery query)
        {
            return Repository
                   .GetAll(new Projector.ToFlat<PaymentType, GetPaymentTypesResponse>());
        }
    }
}