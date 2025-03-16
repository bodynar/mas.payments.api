namespace MAS.Payments.Commands
{
    using System;

    using MAS.Payments.Infrastructure.Command;

    public class AddMeterMeasurementTypeCommand(
        long paymentTypeId,
        string name,
        string description,
        string color
    ) : ICommand
    {
        public string Name { get; } = name ?? throw new ArgumentNullException(nameof(name));

        public string Description { get; } = description;

        public long PaymentTypeId { get; } = paymentTypeId;

        public string Color { get; } = color;
    }
}