using System.Collections.Generic;
using MAS.Payments.DataBase;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Infrastructure.Specification;

namespace MAS.Payments.Queries
{
    public class GetPaymentsQuery : IQuery<IEnumerable<GetPaymentsResponse>>
    {
        public Specification<Payment> Filter { get; }


        public GetPaymentsQuery() { }

        public GetPaymentsQuery(Specification<Payment> filter)
        {
            Filter = filter;
        }
    }
}