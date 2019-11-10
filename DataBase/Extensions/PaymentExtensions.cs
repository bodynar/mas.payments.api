using System;
using System.Linq.Expressions;

namespace MAS.Payments.DataBase
{
    public partial class Payment : Entity, IOwnedEntity<Payment>
    {
        public Expression<Func<Payment, long>> EntityOwnerId => entity => entity.Author.Id;
    }
}