namespace MAS.Payments.Queries
{
    using System.Collections.Generic;

    using MAS.Payments.Infrastructure.Query;

    public class GetPaymentsQuery(
        byte? month,
        short? year,
        Guid? paymentTypeId,
        double? exactAmount,
        double? minAmount,
        double? maxAmount
    ) : IQuery<IEnumerable<GetPaymentsResponse>>
    {
        public byte? Month { get; } = month;

        public short? Year { get; } = year;

        public Guid? PaymentTypeId { get; } = paymentTypeId;

        public double? ExactAmount { get; } = exactAmount;

        public double? MinAmount { get; } = minAmount;

        public double? MaxAmount { get; } = maxAmount;
    }
}
