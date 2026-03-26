namespace MAS.Payments.DataBase
{
    using System;

    public class PaymentGroupTemplateItem : Entity
    {
        public Guid PaymentGroupTemplateId { get; set; }

        public virtual PaymentGroupTemplate PaymentGroupTemplate { get; set; }

        public Guid PaymentTypeId { get; set; }

        public virtual PaymentType PaymentType { get; set; }
    }
}
