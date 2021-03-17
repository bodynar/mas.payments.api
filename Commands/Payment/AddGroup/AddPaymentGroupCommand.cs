namespace MAS.Payments.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MAS.Payments.Infrastructure.Command;

    public class AddPaymentGroupCommand: ICommand
    {
        public DateTime Date { get; }

        public IEnumerable<PaymentGroup> Payments { get; }

        public AddPaymentGroupCommand(DateTime date, IEnumerable<PaymentGroup> payments)
        {
            Date = date;
            Payments = payments ?? Enumerable.Empty<PaymentGroup>();
        }
    }
}
