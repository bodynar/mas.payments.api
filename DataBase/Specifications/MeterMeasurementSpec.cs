namespace MAS.Payments.DataBase
{
    using System;
    using System.Linq.Expressions;

    using MAS.Payments.Infrastructure.Specification;

    public static class MeterMeasurementSpec
    {
        public class WithoutDiff : Specification<MeterMeasurement>
        {
            public override Expression<Func<MeterMeasurement, bool>> IsSatisfied()
                => measurement => measurement.Diff == null || measurement.Diff == 0;
        }
    }
}
