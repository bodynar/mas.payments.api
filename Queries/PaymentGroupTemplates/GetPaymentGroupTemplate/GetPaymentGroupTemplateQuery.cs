namespace MAS.Payments.Queries
{
    using System;

    using MAS.Payments.Infrastructure.Query;

    public class GetPaymentGroupTemplateQuery(
        Guid id
    ) : IQuery<GetPaymentGroupTemplateResponse>
    {
        public Guid Id { get; } = id;
    }
}
