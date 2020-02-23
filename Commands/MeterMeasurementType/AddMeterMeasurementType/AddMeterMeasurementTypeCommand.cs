
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class AddMeterMeasurementTypeCommand : ICommand
    {
        public string Name { get; }

        public string Description { get; }

        public long PaymentTypeId { get; }

        public AddMeterMeasurementTypeCommand(long paymentTypeId, string name, string description)
        {
            Name = name;
            Description = description;
            PaymentTypeId = paymentTypeId;
        }
    }
}