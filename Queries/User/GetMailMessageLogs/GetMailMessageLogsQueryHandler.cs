namespace MAS.Payments.Queries
{
    using System.Collections.Generic;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Projector;
    using MAS.Payments.Infrastructure.Query;

    internal class GetMailMessageLogsQueryHandler : BaseQueryHandler<GetMailMessageLogsQuery, IEnumerable<GetMailMessageLogsQueryResult>>
    {
        private IRepository<MailMessageLogItem> Repository { get; }

        private IProjector<MailMessageLogItem, GetMailMessageLogsQueryResult> Projector { get; }

        public GetMailMessageLogsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<MailMessageLogItem>();
        }

        public override IEnumerable<GetMailMessageLogsQueryResult> Handle(GetMailMessageLogsQuery query)
        {
            return Repository.GetAll(Projector);
        }
    }
}