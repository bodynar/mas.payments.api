using System;
using MAS.Payments.Infrastructure.MailMessaging;

namespace MAS.Payments.MailMessages
{
    public class TestMailMessageModel
    {
        public int Counter { get; }

        public string Name { get; }

        public TestMailMessageModel(int counter, string name)
        {
            Counter = counter;
            Name = name ?? throw new ArgumentException(nameof(name));
        }
    }

    public class TestMailMessageWithModel : IMailMessage<TestMailMessageModel>
    {
        public string TemplateName
            => "TestModel";

        public string Recipient { get; }

        public string Subject
            => "Test mail message";

        public TestMailMessageModel Model { get; }

        public TestMailMessageWithModel(string recipient, int counter, string name)
        {
            Recipient = recipient ?? throw new ArgumentException(nameof(recipient));

            Model = new TestMailMessageModel(counter, name);
        }
    }
}