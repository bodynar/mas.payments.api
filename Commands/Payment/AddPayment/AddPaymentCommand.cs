namespace MAS.Payments.Commands
{
    using System;

    using MAS.Payments.Infrastructure.Command;

    public class AddPaymentCommand(
        long paymentTypeId,
        double amount,
        DateTime? date,
        string description
    ) : ICommand
    {
        public double Amount { get; } = amount;

        public DateTime? Date { get; } = date;

        public string Description { get; } = description;

        public long PaymentTypeId { get; } = paymentTypeId;

        public Func<long> CreatedPaymentIdProvider { get; set; }
    }
}