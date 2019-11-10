using System;
using System.Linq.Expressions;

namespace MAS.Payments.DataBase
{
    public partial class PaymentType : Entity, IOwnedEntity<PaymentType>
    {
        public Expression<Func<PaymentType, long>> EntityOwnerId => entity => entity.Author.Id;
    }
}