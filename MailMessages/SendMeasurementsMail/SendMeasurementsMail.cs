using System;
using System.Collections.Generic;
using MAS.Payments.Infrastructure.MailMessaging;

namespace MAS.Payments.MailMessages
{
    public class SendMeasurementsMail : IMailMessage<IEnumerable<MeasurementMailModel>>
    {
        public string TemplateName
            => "SendMeasurements";

        public string Recipient { get; }

        public string Subject { get; }

        public IEnumerable<MeasurementMailModel> Model { get; }

        public SendMeasurementsMail(string recipient, DateTime date, IEnumerable<MeasurementMailModel> measurements)
        {
            Recipient = recipient ?? throw new ArgumentException(nameof(recipient));
            Subject = $"Measurements for {date.ToString("MMMM.yyyy")}";
            Model = measurements ?? throw new ArgumentException(nameof(measurements));
        }
    }
}