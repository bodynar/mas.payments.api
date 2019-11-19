using System;

using MAS.Payments.Infrastructure.MailMessaging;

namespace MAS.Payments.MailMessages
{
    public class ForgotPasswordMailMessage : IMailMessage<ForgotPasswordModel>
    {
        public string TemplateName
            => "ResetPassword";

        public string Recipient { get; }

        public string Subject
            => "Password reset";

        public ForgotPasswordModel Model { get; }

        public ForgotPasswordMailMessage(string link, string recipient, string token, string firstName, string lastName)
        {
            Recipient = recipient ?? throw new ArgumentException(nameof(recipient));

            Model = new ForgotPasswordModel(link, token, firstName, lastName);
        }
    }
}