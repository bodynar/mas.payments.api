namespace MAS.Payments.Commands
{
    using MAS.Payments.Infrastructure.Command;

    public class DeleteMeterMeasurementCommand(
        Guid meterMeasurementId
    ) : ICommand
    {
        public Guid MeterMeasurementId { get; } = meterMeasurementId;
    }
}
