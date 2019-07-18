
using System;
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class AddPaymentCommand : ICommand
    {
        public double Amount { get; }

        public DateTime? Date { get; }

        public string Description { get; }
        
        public long PaymentTypeID { get; }

        public AddPaymentCommand(
            long paymentTypeID, double amount, DateTime? date,
            string description)
        {
            PaymentTypeID = paymentTypeID;
            Amount = amount;
            Date = date;
            Description = description;
        }
    }
}