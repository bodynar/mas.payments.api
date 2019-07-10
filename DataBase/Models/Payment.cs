using System;

namespace MAS.Payments.DataBase
{
    public class Payment
    {
        public long Id { get; set; }

        public double Amount { get; set; }

        public DateTime? Date { get; set; }

        public string Description { get; set; }

        public PaymentType PaymentType { get; set; }
    }
}