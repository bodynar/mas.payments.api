using System;

namespace MAS.Payments.Infrastructure.MailMessaging
{
    public abstract class BaseMailMessageModel
    {
        public DateTime Today { get; } = DateTime.Today;

        public int CurrentYear =>
            Today.Year;
    }
}
