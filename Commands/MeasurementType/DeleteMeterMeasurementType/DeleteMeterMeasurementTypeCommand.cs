
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class DeleteMeterMeasurementTypeCommand : ICommand
    {
        public long MeterMeasurementTypeId { get; set; }

        public DeleteMeterMeasurementTypeCommand(long meterMeasurementTypeId)
        {
            MeterMeasurementTypeId = meterMeasurementTypeId;
        }
    }
}