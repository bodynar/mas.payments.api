namespace MAS.Payments.DataBase
{
    using System;
    using System.Collections.Generic;

    public class PaymentGroup : Entity
    {
        public DateTime PaymentDate { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public string Comment { get; set; }

        public virtual ICollection<Payment> Payments { get; } = [];

        public virtual PaymentFile PaymentFile { get; set; }
    }
}
