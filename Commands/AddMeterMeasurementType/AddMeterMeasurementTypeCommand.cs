
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class AddMeterMeasurementTypeCommand : ICommand
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public long PaymentTypeId { get; set; }

        public AddMeterMeasurementTypeCommand(long paymentTypeId, string name, string description)
        {
            Name = name;
            Description = description;
            PaymentTypeId = paymentTypeId;
        }
    }
}