using System;
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class UpdatePaymentCommand : ICommand
    {
        public long Id { get; }

        public double Amount { get; }

        public DateTime? Date { get; }

        public string Description { get; }

        public long PaymentTypeId { get; }

        public UpdatePaymentCommand(long id,
            long paymentTypeId, double amount, DateTime? date,
            string description)
        {
            Id = id;
            PaymentTypeId = paymentTypeId;
            Amount = amount;
            Date = date;
            Description = description;
        }
    }
}