namespace MAS.Payments.MailMessages
{
    public class MeasurementMailModel
    {
        public string TypeName { get; }

        public double Measurement { get; }

        public MeasurementMailModel(string typeName, double measurement)
        {
            TypeName = typeName;
            Measurement = measurement;
        }
    }
}