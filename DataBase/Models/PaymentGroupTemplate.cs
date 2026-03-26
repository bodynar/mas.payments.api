namespace MAS.Payments.DataBase
{
    using System.Collections.Generic;

    public class PaymentGroupTemplate : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<PaymentGroupTemplateItem> Items { get; } = [];
    }
}
