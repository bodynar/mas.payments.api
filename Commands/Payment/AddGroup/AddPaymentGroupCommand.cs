namespace MAS.Payments.Commands
{
    using System;
    using System.Collections.Generic;

    using MAS.Payments.Infrastructure.Command;

    using Microsoft.AspNetCore.Http;

    public class AddPaymentGroupCommand(
        DateTime date,
        IEnumerable<PaymentGroup> payments,
        IFormFile receiptFile,
        IFormFile check
    ) : ICommand
    {
        public DateTime Date { get; } = date;

        public IEnumerable<PaymentGroup> Payments { get; } = payments ?? [];

        public IFormFile ReceiptFile { get; } = receiptFile;

        public IFormFile Check { get; } = check;
    }
}
