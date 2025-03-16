namespace MAS.Payments.Commands
{
    using System;
    using System.Collections.Generic;

    using MAS.Payments.Infrastructure.Command;

    public class AddPaymentGroupCommand(
        DateTime date,
        IEnumerable<PaymentGroup> payments
    ) : ICommand
    {
        public DateTime Date { get; } = date;

        public IEnumerable<PaymentGroup> Payments { get; } = payments ?? [];
    }
}
