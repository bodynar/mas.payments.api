namespace MAS.Payments.Commands
{
    using System;

    using MAS.Payments.Infrastructure.Command;

    public class UpdatePaymentGroupCommand(
        long id,
        DateTime paymentDate,
        int month,
        int year,
        string comment
    ) : ICommand
    {
        public long Id { get; } = id;

        public DateTime PaymentDate { get; } = paymentDate;

        public int Month { get; } = month;

        public int Year { get; } = year;

        public string Comment { get; } = comment;
    }
}
