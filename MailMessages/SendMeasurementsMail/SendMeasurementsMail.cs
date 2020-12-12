namespace MAS.Payments.MailMessages
{
    using System;

    using MAS.Payments.Infrastructure.MailMessaging;

    public class SendMeasurementsMail : IMailMessage<MeasurementMailModel>
    {
        public string TemplateName
            => "SendMeasurements";

        public string Recipient { get; }

        public string Subject { get; }

        public MeasurementMailModel Model { get; }

        public SendMeasurementsMail(string recipient, string date, MeasurementMailModel measurementModel)
        {
            Recipient = recipient ?? throw new ArgumentException(nameof(recipient));
            Subject = date ?? throw new ArgumentException(nameof(date));
            Model = measurementModel ?? throw new ArgumentException(nameof(measurementModel));
        }
    }
}