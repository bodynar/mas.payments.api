namespace MAS.Payments.Queries
{
    using System.Collections.Generic;
    
    using MAS.Payments.Infrastructure.Query;

    public class GetMailMessageLogsQuery : IQuery<IEnumerable<GetMailMessageLogsQueryResult>>
    {
        public GetMailMessageLogsQuery()
        {
        }
    }
}