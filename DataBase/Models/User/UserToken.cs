using System;
namespace MAS.Payments.DataBase
{
    public class UserToken : Entity
    {
        public string Token { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ActiveTo { get; set; }

        public long UserTokenTypeId { get; set; }

        public virtual UserTokenType TokenType { get; set; }

        public long UserId { get; set; }

        public virtual User User { get; set; }
    }
}