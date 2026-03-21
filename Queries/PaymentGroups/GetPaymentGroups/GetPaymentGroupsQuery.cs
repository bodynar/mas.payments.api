namespace MAS.Payments.Queries
{
    using System.Collections.Generic;

    using MAS.Payments.Infrastructure.Query;

    public class GetPaymentGroupsQuery(
        byte? month = null,
        short? year = null
    ) : IQuery<IEnumerable<GetPaymentGroupsResponse>>
    {
        public byte? Month { get; } = month;

        public short? Year { get; } = year;
    }
}
