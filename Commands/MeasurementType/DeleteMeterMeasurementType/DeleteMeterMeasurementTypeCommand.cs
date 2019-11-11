
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class DeleteMeterMeasurementTypeCommand : UserCommand
    {
        public long MeterMeasurementTypeId { get; set; }

        public DeleteMeterMeasurementTypeCommand(long userId, long meterMeasurementTypeId)
            : base(userId)
        {
            MeterMeasurementTypeId = meterMeasurementTypeId;
        }
    }
}