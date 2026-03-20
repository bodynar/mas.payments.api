namespace MAS.Payments.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Specification;
    using MAS.Payments.Queries.Measurements;

    using Microsoft.EntityFrameworkCore;

    internal class RecalculateDiffCommandHandler : BaseCommandHandler<RecalculateDiffCommand>
    {
        private IRepository<MeterMeasurement> Repository { get; }

        public RecalculateDiffCommandHandler(IResolver resolver)
            : base(resolver)
        {
            Repository = GetRepository<MeterMeasurement>();
        }

        public override async Task HandleAsync(RecalculateDiffCommand command)
        {
            var specification = command.ForAll
                ? new CommonSpecification<MeterMeasurement>(x => true)
                : new MeterMeasurementSpec.WithoutDiff() as Specification<MeterMeasurement>;

            var measurementItems = await Repository.Where(specification).ToListAsync();

            foreach (var measurementItem in measurementItems)
            {
                var previousItem = await QueryProcessor.Execute(
                    new GetSiblingMeasurementQuery(
                        measurementItem.MeterMeasurementTypeId, measurementItem.Date, GetSiblingMeasurementDirection.Previous
                    )
                );

                if (previousItem != null && previousItem.Measurement < measurementItem.Measurement)
                {
                    measurementItem.Diff = Math.Abs(previousItem.Measurement - measurementItem.Measurement);
                }
            }
        }
    }
}
