using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class UpdateMeterMeasurementTypeCommand : UserCommand
    {
        public long Id { get; }

        public string Name { get; }

        public string Description { get; }

        public long PaymentTypeId { get; }

        public UpdateMeterMeasurementTypeCommand(long userId,
            long id, long paymentTypeId, string name, string description)
            : base(userId)
        {
            Id = id;
            Name = name;
            Description = description;
            PaymentTypeId = paymentTypeId;
        }
    }
}