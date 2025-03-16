namespace MAS.Payments.Commands
{
    public class MeasurementGroup(
        long measurementTypeId,
        double measurement,
        string comment
    )
    {
        public long MeasurementTypeId { get; } = measurementTypeId;

        public double Measurement { get; } = measurement;

        public string Comment { get; } = comment;
    }
}
