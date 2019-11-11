
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class DeleteMeterMeasurementCommand : UserCommand
    {
        public long MeterMeasurementId { get; set; }

        public DeleteMeterMeasurementCommand(long userId, long meterMeasurementId)
            : base(userId)
        {
            MeterMeasurementId = meterMeasurementId;
        }
    }
}