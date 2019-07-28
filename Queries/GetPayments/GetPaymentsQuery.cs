using System.Collections.Generic;
using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Queries
{
    public class GetPaymentsQuery : IQuery<IEnumerable<GetPaymentsResponse>>
    {
        public byte? Month { get; }

        public long? PaymentTypeId { get; }

        public double? ExactAmount { get; }

        public double? MinAmount { get; }

        public double? MaxAmount { get; }

        public GetPaymentsQuery()
        { }

        public GetPaymentsQuery(
            byte? month, long? paymentTypeId,
            double? exactAmount, double? minAmount, double? maxAmount)
        { 
            Month = month;
            PaymentTypeId = paymentTypeId;
            ExactAmount = exactAmount;
            MinAmount = minAmount;
            MaxAmount = maxAmount;
        }
    }
}