using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Infrastructure.Specification;

namespace MAS.Payments.Queries
{
    internal class IsTokenValidQueryHandler : BaseQueryHandler<IsTokenValidQuery, bool>
    {
        private IRepository<UserToken> Repository { get; }

        public IsTokenValidQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<UserToken>();
        }

        public override bool Handle(IsTokenValidQuery query)
        {
            var token =
                Repository.Get(
                    new CommonSpecification<UserToken>(
                        x => x.Token == query.Token && x.UserTokenTypeId == query.UserTokenTypeId));

            if (token != null)
            {
                query.UserToken = token;
            }

            return
                token != null
                && token.ActiveTo >= query.ActiveTo;
        }
    }
}