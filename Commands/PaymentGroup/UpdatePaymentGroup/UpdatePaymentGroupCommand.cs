namespace MAS.Payments.Commands
{
    using System;

    using MAS.Payments.Infrastructure.Command;

    public class UpdatePaymentGroupCommand(
        Guid id,
        DateTime paymentDate,
        int month,
        int year,
        string comment
    ) : ICommand
    {
        public Guid Id { get; } = id;

        public DateTime PaymentDate { get; } = paymentDate;

        /// <summary> Billing period month (1-12). May differ from PaymentDate.Month. </summary>
        public int Month { get; } = month is >= 1 and <= 12 ? month : throw new ArgumentOutOfRangeException(nameof(month));

        /// <summary> Billing period year. May differ from PaymentDate.Year. </summary>
        public int Year { get; } = year > 0 ? year : throw new ArgumentOutOfRangeException(nameof(year));

        public string Comment { get; } = comment;
    }
}
