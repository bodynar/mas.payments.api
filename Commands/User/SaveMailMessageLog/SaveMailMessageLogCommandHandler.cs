namespace MAS.Payments.Commands
{
    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;

    public class SaveMailMessageLogCommandHandler : BaseCommandHandler<SaveMailMessageLogCommand>
    {
        private IRepository<MailMessageLogItem> Repository { get; }

        public SaveMailMessageLogCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<MailMessageLogItem>();
        }

        public override void Handle(SaveMailMessageLogCommand command)
        {
            Repository.Add(new MailMessageLogItem
            {
                Body = command.Body,
                Recipient = command.Recipient,
                Subject = command.Subject,
                SentDate = command.SentDate,
            });
        }
    }
}