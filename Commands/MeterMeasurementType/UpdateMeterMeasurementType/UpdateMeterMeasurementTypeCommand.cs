namespace MAS.Payments.Commands
{
    using MAS.Payments.Infrastructure.Command;

    public class UpdateMeterMeasurementTypeCommand(
        Guid id,
        Guid paymentTypeId,
        string name,
        string description,
        string color
    ) : ICommand
    {
        public Guid Id { get; } = id;

        public string Name { get; } = name;

        public string Description { get; } = description;

        public Guid PaymentTypeId { get; } = paymentTypeId;

        public string Color { get; } = color;
    }
}
