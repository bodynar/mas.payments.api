using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class UpdateMeterMeasurementTypeCommand : ICommand
    {
        public long Id { get; }

        public string Name { get; }

        public string Description { get; }

        public long PaymentTypeId { get; }

        public UpdateMeterMeasurementTypeCommand(long id, long paymentTypeId, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
            PaymentTypeId = paymentTypeId;
        }
    }
}