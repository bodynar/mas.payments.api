namespace MAS.Payments.Queries
{
    using System;
    using System.Collections.Generic;

    public class GetPaymentGroupResponse
    {
        public Guid Id { get; set; }

        public DateTime PaymentDate { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public string Comment { get; set; }

        public ICollection<GetPaymentGroupPaymentResponse> Payments { get; set; } = [];
    }

    public class GetPaymentGroupPaymentResponse
    {
        public Guid Id { get; set; }

        public double Amount { get; set; }

        public string Description { get; set; }

        public Guid PaymentTypeId { get; set; }

        public string PaymentTypeName { get; set; }

        public string PaymentTypeColor { get; set; }
    }
}
