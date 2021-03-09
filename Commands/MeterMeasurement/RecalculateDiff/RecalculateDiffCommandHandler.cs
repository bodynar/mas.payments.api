namespace MAS.Payments.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Specification;
    using MAS.Payments.Queries.Measurements;

    internal class RecalculateDiffCommandHandler : BaseCommandHandler<RecalculateDiffCommand>
    {
        private IRepository<MeterMeasurement> Repository { get; }

        public RecalculateDiffCommandHandler(IResolver resolver)
            : base(resolver)
        {
            Repository = GetRepository<MeterMeasurement>();
        }

        public override void Handle(RecalculateDiffCommand command)
        {
            var specification = command.ForAll
                ? new CommonSpecification<MeterMeasurement>(x => true)
                : new MeterMeasurementSpec.WithoutDiff() as Specification<MeterMeasurement>;

            var measurementItems = Repository.Where(specification).ToList();
            var warnings = new List<string>();

            foreach (var measurementItem in measurementItems)
            {
                var previousItem = QueryProcessor.Execute(new GetSiblingMeasurementQuery(measurementItem.MeterMeasurementTypeId, measurementItem.Date, GetSiblingMeasurementDirection.Previous));

                if (previousItem != null)
                {
                    if (previousItem.Measurement >= measurementItem.Measurement)
                    {
                        warnings.Add($"[{measurementItem.MeasurementType.Name} - {measurementItem.Date:MMMM yyyy}]: Value is less than previous one");
                    }
                    else
                    {
                        measurementItem.Diff = Math.Abs(previousItem.Measurement - measurementItem.Measurement);
                    }
                }
                else
                {
                    warnings.Add($"[{measurementItem.MeasurementType.Name} - {measurementItem.Date:MMMM yyyy}]: Previous measurement not found");
                }
            }

            if (warnings.Any())
            {
                command.Warnings = warnings;
            }
        }
    }
}
