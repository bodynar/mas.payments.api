using MAS.Payments.Infrastructure.MailMessaging;

namespace MAS.Payments.MailMessages
{
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