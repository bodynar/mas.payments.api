using System;
using System.Linq.Expressions;

namespace MAS.Payments.DataBase
{
    public partial class MeterMeasurement : Entity, IOwnedEntity<MeterMeasurement>
    {
        public Expression<Func<MeterMeasurement, long>> EntityOwnerId => entity => entity.Author.Id;
    }
}