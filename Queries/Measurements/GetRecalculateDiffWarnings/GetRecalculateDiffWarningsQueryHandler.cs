namespace MAS.Payments.Queries.Measurements
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;
    using MAS.Payments.Infrastructure.Specification;

    using Microsoft.EntityFrameworkCore;

    internal class GetRecalculateDiffWarningsQueryHandler : BaseQueryHandler<GetRecalculateDiffWarningsQuery, IEnumerable<string>>
    {
        private IRepository<MeterMeasurement> Repository { get; }

        public GetRecalculateDiffWarningsQueryHandler(IResolver resolver)
            : base(resolver)
        {
            Repository = GetRepository<MeterMeasurement>();
        }

        public override async Task<IEnumerable<string>> HandleAsync(GetRecalculateDiffWarningsQuery query)
        {
            var specification = query.ForAll
                ? new CommonSpecification<MeterMeasurement>(x => true)
                : new MeterMeasurementSpec.WithoutDiff() as Specification<MeterMeasurement>;

            var measurementItems = await Repository.Where(specification).ToListAsync();
            var warnings = new List<string>();

            foreach (var measurementItem in measurementItems)
            {
                var previousItem = await QueryProcessor.Execute(
                    new GetSiblingMeasurementQuery(
                        measurementItem.MeterMeasurementTypeId, measurementItem.Date, GetSiblingMeasurementDirection.Previous
                    )
                );

                if (previousItem != null)
                {
                    if (previousItem.Measurement >= measurementItem.Measurement)
                    {
                        warnings.Add($"[{measurementItem.MeterMeasurementTypeId} - {measurementItem.Date:MMMM yyyy}]: Value is less than previous one");
                    }
                }
                else
                {
                    warnings.Add($"[{measurementItem.MeterMeasurementTypeId} - {measurementItem.Date:MMMM yyyy}]: Previous measurement not found");
                }
            }

            return warnings;
        }
    }
}
