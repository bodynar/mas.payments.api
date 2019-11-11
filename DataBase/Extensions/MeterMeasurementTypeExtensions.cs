using System;
using System.Linq.Expressions;

namespace MAS.Payments.DataBase
{
    public partial class MeterMeasurementType : OwnedEntity<MeterMeasurementType>
    {
        public override Expression<Func<MeterMeasurementType, long>> EntityOwnerId => entity => entity.Author.Id;
    }
}