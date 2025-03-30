namespace MAS.Payments.Commands
{
    using System;

    using MAS.Payments.Infrastructure.Command;
    using Microsoft.AspNetCore.Http;

    public class AddPaymentCommand(
        long paymentTypeId,
        double amount,
        DateTime? date,
        string description,
        IFormFile receiptFile,
        IFormFile check
    ) : ICommand
    {
        public double Amount { get; } = amount;

        public DateTime? Date { get; } = date;

        public string Description { get; } = description;

        public long PaymentTypeId { get; } = paymentTypeId;

        public IFormFile ReceiptFile { get; } = receiptFile;

        public IFormFile Check { get; } = check;
    }
}