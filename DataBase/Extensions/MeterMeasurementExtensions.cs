using System;
using System.Linq.Expressions;

namespace MAS.Payments.DataBase
{
    public partial class MeterMeasurement : OwnedEntity<MeterMeasurement>
    {
        public override Expression<Func<MeterMeasurement, long>> EntityOwnerId => entity => entity.Author.Id;
    }
}