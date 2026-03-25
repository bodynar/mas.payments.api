namespace MAS.Payments.Commands
{
    using System;
    using System.Collections.Generic;

    using MAS.Payments.Infrastructure.Command;

    public class UpdatePaymentGroupTemplateCommand(
        Guid id,
        string name,
        string description,
        IEnumerable<Guid> paymentTypeIds
    ) : ICommand
    {
        public Guid Id { get; } = id;

        public string Name { get; } = name;

        public string Description { get; } = description;

        public IEnumerable<Guid> PaymentTypeIds { get; } = paymentTypeIds ?? [];
    }
}
