namespace MAS.Payments.Commands
{
    using MAS.Payments.Infrastructure.Command;

    public class UpdateMeterMeasurementTypeCommand(
        long id,
        long paymentTypeId,
        string name,
        string description,
        string color
    ) : ICommand
    {
        public long Id { get; } = id;

        public string Name { get; } = name;

        public string Description { get; } = description;

        public long PaymentTypeId { get; } = paymentTypeId;

        public string Color { get; } = color;
    }
}