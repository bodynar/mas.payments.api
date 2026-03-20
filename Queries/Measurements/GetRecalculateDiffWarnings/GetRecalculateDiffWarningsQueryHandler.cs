namespace MAS.Payments.Queries.Measurements
{
    using System;
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

            if (measurementItems.Count == 0)
            {
                return [];
            }

            var typeIds = measurementItems.Select(m => m.MeterMeasurementTypeId).Distinct().ToList();
            var prevDates = measurementItems.Select(m => m.Date.AddMonths(-1)).ToList();
            var minDate = new DateTime(prevDates.Min().Year, prevDates.Min().Month, 1);
            var maxDate = new DateTime(prevDates.Max().Year, prevDates.Max().Month, 1).AddMonths(1);

            var previousMeasurements = await Repository
                .Where(new CommonSpecification<MeterMeasurement>(x =>
                    typeIds.Contains(x.MeterMeasurementTypeId)
                    && x.Date >= minDate
                    && x.Date < maxDate))
                .ToListAsync();

            var previousLookup = previousMeasurements
                .ToLookup(x => (x.MeterMeasurementTypeId, x.Date.Year, x.Date.Month));

            var warnings = new List<string>();

            foreach (var measurementItem in measurementItems)
            {
                var prevDate = measurementItem.Date.AddMonths(-1);
                var key = (measurementItem.MeterMeasurementTypeId, prevDate.Year, prevDate.Month);

                var previousItem = previousLookup[key]
                    .OrderByDescending(x => x.Date)
                    .FirstOrDefault();

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
