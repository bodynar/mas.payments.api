using System;
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class UpdatePaymentCommand : UserCommand
    {
        public long Id { get; }

        public double Amount { get; }

        public DateTime? Date { get; }

        public string Description { get; }

        public long PaymentTypeId { get; }

        public UpdatePaymentCommand(long userId,
            long id, long paymentTypeId, double amount, DateTime? date,
            string description)
            : base(userId)
        {
            Id = id;
            PaymentTypeId = paymentTypeId;
            Amount = amount;
            Date = date;
            Description = description;
        }
    }
}