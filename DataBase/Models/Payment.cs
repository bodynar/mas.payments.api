using System;

namespace MAS.Payments.DataBase
{
    public partial class Payment
    {
        public double Amount { get; set; }

        public DateTime? Date { get; set; }

        public string Description { get; set; }

        public virtual User Author { get; set; }

        public long? AuthorId { get; set; }

        public long PaymentTypeId { get; set; }

        public virtual PaymentType PaymentType { get; set; }
    }
}