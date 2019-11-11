using System;
using System.Linq.Expressions;

namespace MAS.Payments.DataBase
{
    public partial class Payment : OwnedEntity<Payment>
    {
        public override Expression<Func<Payment, long?>> EntityOwnerId => entity => entity.AuthorId;
    }
}