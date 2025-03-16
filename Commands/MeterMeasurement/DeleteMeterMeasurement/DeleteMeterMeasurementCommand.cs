namespace MAS.Payments.Commands
{
    using MAS.Payments.Infrastructure.Command;

    public class DeleteMeterMeasurementCommand(
        long meterMeasurementId
    ) : ICommand
    {
        public long MeterMeasurementId { get; } = meterMeasurementId;
    }
}