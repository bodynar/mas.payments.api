namespace MAS.Payments.Queries.Measurements
{
    using System.Linq;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;
    using MAS.Payments.Infrastructure.Specification;

    using Microsoft.EntityFrameworkCore;

    internal class GetSiblingMeasurementHandler : BaseQueryHandler<GetSiblingMeasurementQuery, MeterMeasurement>
    {
        private IRepository<MeterMeasurement> Repository { get; }

        public GetSiblingMeasurementHandler(IResolver resolver)
            : base(resolver)
        {
            Repository = GetRepository<MeterMeasurement>();
        }

        public override async Task<MeterMeasurement> HandleAsync(GetSiblingMeasurementQuery query)
        {
            var targetDate = query.Date.AddMonths(query.Direction == GetSiblingMeasurementDirection.Next ? 1 : -1);

            return await Repository
                .Where(
                    new CommonSpecification<MeterMeasurement>(x =>
                        x.MeterMeasurementTypeId == query.TypeId
                        && x.Date.Year == targetDate.Year
                        && x.Date.Month == targetDate.Month
                    )
                )
                .OrderByDescending(x => x.Date)
                .FirstOrDefaultAsync();
        }
    }
}
