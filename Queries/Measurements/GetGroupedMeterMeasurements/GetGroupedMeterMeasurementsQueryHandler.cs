namespace MAS.Payments.Queries
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

    internal class GetGroupedMeterMeasurementsQueryHandler : BaseQueryHandler<GetGroupedMeterMeasurementsQuery, IEnumerable<GetGroupedMeterMeasurementsResponse>>
    {
        private IRepository<MeterMeasurement> Repository { get; }

        public GetGroupedMeterMeasurementsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<MeterMeasurement>();
        }

        public override async Task<IEnumerable<GetGroupedMeterMeasurementsResponse>> HandleAsync(GetGroupedMeterMeasurementsQuery query)
        {
            Specification<MeterMeasurement> filter = new CommonSpecification<MeterMeasurement>(x => true);

            if (query.Month.HasValue)
            {
                filter = filter && new CommonSpecification<MeterMeasurement>(x => x.Date.Month == query.Month.Value);
            }

            if (query.Year.HasValue)
            {
                filter = filter && new CommonSpecification<MeterMeasurement>(x => x.Date.Year == query.Year.Value);
            }

            if (query.MeterMeasurementTypeId.HasValue)
            {
                filter = filter && new CommonSpecification<MeterMeasurement>(x => x.MeterMeasurementTypeId == query.MeterMeasurementTypeId.Value);
            }

            var filteredMeasurements =
                await Repository
                   .Where(filter)
                   .ToListAsync();

            var result = new List<GetGroupedMeterMeasurementsResponse>();

            foreach (var measurement in filteredMeasurements)
            {
                var group = result.FirstOrDefault(x => x.DateMonth == measurement.Date.Month && x.DateYear == measurement.Date.Year);

                if (group != null)
                {
                    group.Measurements.Add(
                        new GetGroupedMeterMeasurementsResponseMeasurement
                        {
                            Id = measurement.Id,
                            DateYear = measurement.Date.Year,
                            DateMonth = measurement.Date.Month,
                            Measurement = measurement.Measurement,
                            Comment = measurement.Comment,
                            IsSent = measurement.IsSent,
                            MeterMeasurementTypeId = measurement.MeterMeasurementTypeId,
                            MeasurementTypeColor = measurement.MeasurementType.Color,
                            MeasurementTypeName = measurement.MeasurementType.Name,
                        }    
                    );
                } else
                {
                    group = new GetGroupedMeterMeasurementsResponse
                    {
                        DateMonth = measurement.Date.Month,
                        DateYear = measurement.Date.Year,
                    };
                    group.Measurements.Add(
                        new GetGroupedMeterMeasurementsResponseMeasurement
                        {
                            Id = measurement.Id,
                            DateYear = measurement.Date.Year,
                            DateMonth = measurement.Date.Month,
                            Measurement = measurement.Measurement,
                            Comment = measurement.Comment,
                            IsSent = measurement.IsSent,
                            MeterMeasurementTypeId = measurement.MeterMeasurementTypeId,
                            MeasurementTypeColor = measurement.MeasurementType.Color,
                            MeasurementTypeName = measurement.MeasurementType.Name,
                        }
                    );

                    result.Add(group);
                }
            }

            result.ForEach(x => x.SortMeasurements());

            return result.OrderBy(x => x.DateYear).ThenBy(x => x.DateMonth);
        }
    }
}