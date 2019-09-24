using System;
using MAS.Payments.Infrastructure.MailMessaging;

namespace MAS.Payments.MailMessages
{
    public class ConfirmRegistrationMailMessage : IMailMessage<ConfirmRegistrationModel>
    {
        public string TemplateName
            => "ConfirmRegistration";

        public string Recipient { get; }

        public string Subject
            => "Complete registration";

        public ConfirmRegistrationModel Model { get; }

        public ConfirmRegistrationMailMessage(string recipient, string token, string firstName, string lastName)
        {
            Recipient = recipient ?? throw new ArgumentException(nameof(recipient));

            Model = new ConfirmRegistrationModel(token, firstName, lastName);
        }
    }
}