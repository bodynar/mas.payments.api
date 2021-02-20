namespace MAS.Payments.Commands
{
    public class MeasurementGroup
    {
        public long MeasurementTypeId { get; }

        public double Measurement { get; }

        public string Comment { get; }

        public MeasurementGroup(long measurementTypeId, double measurement, string comment)
        {
            MeasurementTypeId = measurementTypeId;
            Measurement = measurement;
            Comment = comment;
        }
    }
}
