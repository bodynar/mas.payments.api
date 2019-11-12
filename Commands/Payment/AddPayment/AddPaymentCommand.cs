
using System;
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class AddPaymentCommand : BaseUserCommand
    {
        public double Amount { get; }

        public DateTime? Date { get; }

        public string Description { get; }
        
        public long PaymentTypeId { get; }

        public AddPaymentCommand(long userId,
            long paymentTypeId, double amount, DateTime? date,
            string description)
            : base(userId)
        {
            PaymentTypeId = paymentTypeId;
            Amount = amount;
            Date = date;
            Description = description;
        }
    }
}