namespace MAS.Payments.Models
{
    using System;
    using System.Collections.Generic;

    public class UpdatePaymentGroupTemplateRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<Guid> PaymentTypeIds { get; set; }
    }
}
