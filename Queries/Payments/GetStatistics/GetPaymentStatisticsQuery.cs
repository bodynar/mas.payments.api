namespace MAS.Payments.Queries
{
    using System;

    using MAS.Payments.Infrastructure.Query;

    public class GetPaymentStatisticsQuery(
        DateTime? from,
        DateTime? to,
        Guid? paymentTypeId
    ) : IQuery<GetPaymentStatisticsResponse>
    {
        public DateTime? From { get; set; } = from;

        public DateTime? To { get; set; } = to;

        public Guid? PaymentTypeId { get; } = paymentTypeId;
    }
}
