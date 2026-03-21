namespace MAS.Payments.Queries
{
    using System;
    using System.Collections.Generic;

    public class GetPaymentGroupsResponse
    {
        public long Id { get; set; }

        public DateTime PaymentDate { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public string Comment { get; set; }

        public int PaymentsCount { get; set; }

        public double TotalAmount { get; set; }
    }
}
