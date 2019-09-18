using System;

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
}