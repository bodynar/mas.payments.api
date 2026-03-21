namespace MAS.Payments.Commands
{
    using System;

    using MAS.Payments.Infrastructure.Command;

    public class UpdatePaymentCommand(
        Guid id,
        Guid paymentTypeId,
        double amount,
        DateTime? date,
        string description
    ) : ICommand
    {
        public Guid Id { get; } = id;

        public double Amount { get; } = amount;

        public DateTime? Date { get; } = date;

        public string Description { get; } = description;

        public Guid PaymentTypeId { get; } = paymentTypeId;
    }
}
