namespace MAS.Payments.Queries.Measurements
{
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;
    using MAS.Payments.Infrastructure.Specification;

    internal class GetSiblingMeasurementHandler : BaseQueryHandler<GetSiblingMeasurementQuery, MeterMeasurement>
    {
        private IRepository<MeterMeasurement> Repository { get; }

        public GetSiblingMeasurementHandler(IResolver resolver)
            : base(resolver)
        {
            Repository = GetRepository<MeterMeasurement>();
        }

        public override MeterMeasurement Handle(GetSiblingMeasurementQuery query)
        {
            var targetDate = query.Date.AddMonths(query.Direction == GetSiblingMeasurementDirection.Next ? 1 : -1);

            return Repository.Where(
                    new CommonSpecification<MeterMeasurement>(x =>
                        x.MeterMeasurementTypeId == query.TypeId
                        && x.Date.Year == targetDate.Year
                        && x.Date.Month == targetDate.Month
                    )
                ).OrderByDescending(x => x.Date)
                .FirstOrDefault();
        }
    }
}
