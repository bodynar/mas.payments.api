namespace MAS.Payments.Commands
{

    using MAS.Payments.Infrastructure.Command;

    public class DeleteMeterMeasurementTypeCommand : ICommand
    {
        public long MeterMeasurementTypeId { get; set; }

        public DeleteMeterMeasurementTypeCommand(long meterMeasurementTypeId)
        {
            MeterMeasurementTypeId = meterMeasurementTypeId;
        }
    }
}