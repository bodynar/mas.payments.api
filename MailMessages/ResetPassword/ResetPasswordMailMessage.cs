using System;

using MAS.Payments.Infrastructure.MailMessaging;

namespace MAS.Payments.MailMessages
{
    public class ResetPasswordMailMessage : IMailMessage<ResetPasswordModel>
    {
        public string TemplateName
            => "ResetPassword";

        public string Recipient { get; }

        public string Subject
            => "Password reset";

        public ResetPasswordModel Model { get; }

        public ResetPasswordMailMessage(string link, string recipient, string token, string firstName, string lastName)
        {
            Recipient = recipient ?? throw new ArgumentException(nameof(recipient));

            Model = new ResetPasswordModel(link, token, firstName, lastName);
        }
    }
}