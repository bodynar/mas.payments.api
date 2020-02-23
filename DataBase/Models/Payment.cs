using System;

namespace MAS.Payments.DataBase
{
    public class Payment : Entity
    {
        public double Amount { get; set; }

        public DateTime? Date { get; set; }

        public string Description { get; set; }

        public long PaymentTypeId { get; set; }

        public virtual PaymentType PaymentType { get; set; }
    }
}