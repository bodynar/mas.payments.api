namespace MAS.Payments.Queries
{
    using System.Collections.Generic;

    using MAS.Payments.Infrastructure.Query;

    public class GetPaymentsQuery : IQuery<IEnumerable<GetPaymentsResponse>>
    {
        public byte? Month { get; }

        public short? Year { get; }

        public long? PaymentTypeId { get; }

        public double? ExactAmount { get; }

        public double? MinAmount { get; }

        public double? MaxAmount { get; }

        public GetPaymentsQuery()
        { }

        public GetPaymentsQuery(
            byte? month, short? year, long? paymentTypeId,
            double? exactAmount, double? minAmount, double? maxAmount)
        { 
            Month = month;
            Year = year;
            PaymentTypeId = paymentTypeId;
            ExactAmount = exactAmount;
            MinAmount = minAmount;
            MaxAmount = maxAmount;
        }
    }
}