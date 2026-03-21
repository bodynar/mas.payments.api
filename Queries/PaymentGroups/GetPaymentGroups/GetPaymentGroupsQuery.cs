namespace MAS.Payments.Queries
{
    using System.Collections.Generic;

    using MAS.Payments.Infrastructure.Query;

    public class GetPaymentGroupsQuery(
        int? month = null,
        int? year = null
    ) : IQuery<IEnumerable<GetPaymentGroupsResponse>>
    {
        public int? Month { get; } = month;

        public int? Year { get; } = year;
    }
}
