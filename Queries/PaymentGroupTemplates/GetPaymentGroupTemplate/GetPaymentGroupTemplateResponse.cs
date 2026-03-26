namespace MAS.Payments.Queries
{
    using System;
    using System.Collections.Generic;

    public class GetPaymentGroupTemplateResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<GetPaymentGroupTemplateItemResponse> PaymentTypes { get; set; } = [];
    }

    public class GetPaymentGroupTemplateItemResponse
    {
        public Guid PaymentTypeId { get; set; }

        public string PaymentTypeName { get; set; }

        public string PaymentTypeColor { get; set; }

        public string PaymentTypeCompany { get; set; }
    }
}
