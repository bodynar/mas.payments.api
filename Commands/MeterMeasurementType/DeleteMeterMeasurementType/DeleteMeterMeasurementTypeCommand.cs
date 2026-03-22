namespace MAS.Payments.Commands
{
    using MAS.Payments.Infrastructure.Command;

    public class DeleteMeterMeasurementTypeCommand(
        Guid meterMeasurementTypeId
    ) : ICommand
    {
        public Guid MeterMeasurementTypeId { get; set; } = meterMeasurementTypeId;
    }
}
