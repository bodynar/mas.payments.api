using System;

namespace MAS.Payments.Queries
{
    public class GetPaymentsQueryResponse
    {
        public long Id { get; set; }
        
        public double Amount { get; set; }

        public DateTime? Date { get; set; }

        public string Description { get; set; }

        public string PaymentType { get; set; }

        public long PaymentTypeId { get; set; }
    }
}