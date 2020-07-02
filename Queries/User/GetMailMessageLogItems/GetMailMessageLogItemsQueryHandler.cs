namespace MAS.Payments.Queries
{
    using System.Collections.Generic;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;
    using MAS.Payments.Projectors;

    internal class GetMailMessageLogItemsQueryHandler : BaseQueryHandler<GetMailMessageLogItemsQuery, IEnumerable<GetMailMessageLogItemsQueryResult>>
    {
        private IRepository<MailMessageLogItem> Repository { get; }

        public GetMailMessageLogItemsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<MailMessageLogItem>();
        }

        public override IEnumerable<GetMailMessageLogItemsQueryResult> Handle(GetMailMessageLogItemsQuery query) 
            => Repository.GetAll(new Projector.ToFlat<MailMessageLogItem, GetMailMessageLogItemsQueryResult>());
    }
}