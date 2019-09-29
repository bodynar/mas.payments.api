using System;

namespace MAS.Payments.MailMessages
{
    public class ConfirmRegistrationModel
    {
        public string FirstName { get; }

        public string LastName { get; }

        public string Token { get; }

        public string Link { get; }

        public string Date { get; }

        public ConfirmRegistrationModel(string link, string token, string firstName, string lastName)
        {
            Link = link ?? throw new ArgumentException(nameof(Link));
            Token = token ?? throw new ArgumentException(nameof(Token));
            FirstName = firstName;
            LastName = lastName;
            Date = DateTime.UtcNow.ToShortDateString();
        }
    }
}