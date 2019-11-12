using System.Collections.Generic;
using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Queries
{
    public class GetPaymentsQuery : BaseUserQuery<IEnumerable<GetPaymentsResponse>>
    {
        public byte? Month { get; }

        public long? PaymentTypeId { get; }

        public double? ExactAmount { get; }

        public double? MinAmount { get; }

        public double? MaxAmount { get; }

        public GetPaymentsQuery(long userId,
            byte? month, long? paymentTypeId,
            double? exactAmount, double? minAmount, double? maxAmount)
            : base(userId)
        { 
            Month = month;
            PaymentTypeId = paymentTypeId;
            ExactAmount = exactAmount;
            MinAmount = minAmount;
            MaxAmount = maxAmount;
        }
    }
}