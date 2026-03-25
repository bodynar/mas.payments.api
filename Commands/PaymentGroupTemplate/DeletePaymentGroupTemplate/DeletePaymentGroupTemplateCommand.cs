namespace MAS.Payments.Commands
{
    using System;

    using MAS.Payments.Infrastructure.Command;

    public class DeletePaymentGroupTemplateCommand(
        Guid id
    ) : ICommand
    {
        public Guid Id { get; } = id;
    }
}
