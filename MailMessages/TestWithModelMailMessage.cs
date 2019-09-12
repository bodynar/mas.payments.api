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

        public string Reciepent { get; }

        public string Subject
            => "Test mail message";

        public TestMailMessageModel Model { get; }

        public TestMailMessageWithModel(string reciepent, int counter, string name)
        {
            Reciepent = reciepent ?? throw new ArgumentException(nameof(reciepent));

            Model = new TestMailMessageModel(counter, name);
        }
    }
}