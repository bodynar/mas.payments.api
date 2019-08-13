
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class DeleteMeterMeasurementCommand : ICommand
    {
        public long MeterMeasurementId { get; set; }

        public DeleteMeterMeasurementCommand(long meterMeasurementId)
        {
            MeterMeasurementId = meterMeasurementId;
        }
    }
}