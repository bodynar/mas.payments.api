namespace MAS.Payments.Queries
{
    using System;

    using MAS.Payments.Infrastructure.Query;

    public class GetPaymentStatisticsQuery : IQuery<GetPaymentStatisticsResponse>
    {
        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public long? PaymentTypeId { get; }

        public GetPaymentStatisticsQuery(DateTime? from, DateTime? to, long? paymentTypeId)
        {
            PaymentTypeId = paymentTypeId;
            From = from;
            To = to;
        }
    }
}