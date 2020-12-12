namespace MAS.Payments.Commands
{
    using System;

    using MAS.Payments.Infrastructure.Command;

    public class AddPaymentCommand : ICommand
    {
        public double Amount { get; }

        public DateTime? Date { get; }

        public string Description { get; }
        
        public long PaymentTypeId { get; }

        public AddPaymentCommand(
            long paymentTypeId, double amount, DateTime? date,
            string description)
        {
            PaymentTypeId = paymentTypeId;
            Amount = amount;
            Date = date;
            Description = description;
        }
    }
}