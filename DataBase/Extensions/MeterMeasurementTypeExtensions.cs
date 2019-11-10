using System;
using System.Linq.Expressions;

namespace MAS.Payments.DataBase
{
    public partial class MeterMeasurementType : Entity, IOwnedEntity<MeterMeasurementType>
    {
        public Expression<Func<MeterMeasurementType, long>> EntityOwnerId => entity => entity.Author.Id;
    }
}