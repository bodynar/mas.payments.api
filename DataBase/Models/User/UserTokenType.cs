using System.Collections.Generic;

namespace MAS.Payments.DataBase
{
    public class UserTokenType : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual IEnumerable<UserToken> UserTokens { get; set; }

        public UserTokenType()
        {
            UserTokens = new List<UserToken>();
        }
    }
}