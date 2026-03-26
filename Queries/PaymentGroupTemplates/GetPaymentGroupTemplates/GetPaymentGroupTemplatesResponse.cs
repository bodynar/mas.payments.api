namespace MAS.Payments.Queries
{
    using System;
    using System.Collections.Generic;

    public class GetPaymentGroupTemplatesResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int PaymentTypesCount { get; set; }

        public ICollection<GetPaymentGroupTemplateItemResponse> PaymentTypes { get; set; } = [];
    }
}
