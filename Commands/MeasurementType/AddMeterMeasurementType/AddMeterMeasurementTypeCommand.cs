
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class AddMeterMeasurementTypeCommand : UserCommand
    {
        public string Name { get; }

        public string Description { get; }

        public long PaymentTypeId { get; }

        public AddMeterMeasurementTypeCommand(long userId, long paymentTypeId, string name, string description)
            : base(userId)
        {
            Name = name;
            Description = description;
            PaymentTypeId = paymentTypeId;
        }
    }
}