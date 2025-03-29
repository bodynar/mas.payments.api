namespace MAS.Payments.Commands
{
    using MAS.Payments.Infrastructure.Command;

    public class DeleteMeterMeasurementTypeCommand(
        long meterMeasurementTypeId
    ) : ICommand
    {
        public long MeterMeasurementTypeId { get; set; } = meterMeasurementTypeId;
    }
}