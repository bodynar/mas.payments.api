namespace MAS.Payments.Commands
{
    public class MeasurementGroup(
        Guid measurementTypeId,
        double measurement,
        string comment
    )
    {
        public Guid MeasurementTypeId { get; } = measurementTypeId;

        public double Measurement { get; } = measurement;

        public string Comment { get; } = comment;
    }
}
