namespace MAS.Payments.Queries.Measurements
{
    using System.Collections.Generic;

    using MAS.Payments.Infrastructure.Query;

    public class GetRecalculateDiffWarningsQuery(
        bool forAll = false
    ) : IQuery<IEnumerable<string>>
    {
        public bool ForAll { get; } = forAll;
    }
}
