using System;
using System.Linq.Expressions;

namespace MAS.Payments.DataBase
{
    public partial class PaymentType : OwnedEntity<PaymentType>
    {
        public override Expression<Func<PaymentType, long?>> EntityOwnerId => entity => entity.AuthorId;
    }
}