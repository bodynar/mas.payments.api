using System.Collections.Generic;
using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Queries
{
    public class GetUsersQuery : IQuery<IEnumerable<GetUsersQueryResponse>>
    {
        public GetUsersQuery()
        {
        }
    }
}