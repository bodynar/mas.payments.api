namespace MAS.Payments.Commands
{

    using System;

    using MAS.Payments.Infrastructure.Command;

    public class AddMeterMeasurementTypeCommand : ICommand
    {
        public string Name { get; }

        public string Description { get; }

        public long PaymentTypeId { get; }

        public string Color { get; }

        public AddMeterMeasurementTypeCommand(long paymentTypeId, string name, string description, string color)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            PaymentTypeId = paymentTypeId;
            Color = color;
        }
    }
}