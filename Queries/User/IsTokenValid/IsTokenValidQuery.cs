using System;
using MAS.Payments.DataBase;
using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Queries
{
    public class IsTokenValidQuery : IQuery<bool>
    {
        public UserToken UserToken { get; set; }

        public string Token { get; }

        public UserTokenTypeEnum TokenTypeEnum { get; }

        public long UserTokenTypeId
            => (long)TokenTypeEnum;

        public DateTime ActiveTo { get; }

        public IsTokenValidQuery(string token, UserTokenTypeEnum tokenType)
        {
            Token = token;
            TokenTypeEnum = tokenType;
            ActiveTo = DateTime.Now;
        }
    }
}