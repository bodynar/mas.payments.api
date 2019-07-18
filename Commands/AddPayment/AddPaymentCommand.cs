
using System;
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class AddPaymentCommand : ICommand
    {
        public double Amount { get; set; }

        public DateTime? Date { get; set; }

        public string Description { get; set; }
        
        public long PaymentTypeID { get; set; }

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