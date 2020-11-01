namespace MAS.Payments.MailMessages
{
    using MAS.Payments.Infrastructure.MailMessaging;

    public class MeasurementMailModel: BaseMailMessageModel
    {
        public string Date { get; }

        public string Measurements { get; }

        public MeasurementMailModel(string date, string measurements)
        {
            Date = date;
            Measurements = measurements;
        }
    }
}