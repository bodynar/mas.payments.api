namespace MAS.Payments.Commands
{
    using System;

    using MAS.Payments.Infrastructure.Command;

    public class AddPaymentCommand(
        Guid paymentTypeId,
        double amount,
        DateTime? date,
        string description
    ) : ICommand
    {
        public double Amount { get; } = amount;

        public DateTime? Date { get; } = date;

        public string Description { get; } = description;

        public Guid PaymentTypeId { get; } = paymentTypeId;
    }
}
