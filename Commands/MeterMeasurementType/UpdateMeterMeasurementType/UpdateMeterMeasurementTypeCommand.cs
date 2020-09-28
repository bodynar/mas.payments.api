namespace MAS.Payments.Commands
{
    using MAS.Payments.Infrastructure.Command;

    public class UpdateMeterMeasurementTypeCommand : ICommand
    {
        public long Id { get; }

        public string Name { get; }

        public string Description { get; }

        public long PaymentTypeId { get; }

        public string Color { get; }

        public UpdateMeterMeasurementTypeCommand(long id, long paymentTypeId, string name, string description, string color)
        {
            Id = id;
            Name = name;
            Description = description;
            PaymentTypeId = paymentTypeId;
            Color = color;
        }
    }
}