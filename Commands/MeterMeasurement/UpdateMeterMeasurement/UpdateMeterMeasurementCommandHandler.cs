namespace MAS.Payments.Commands
{
    using System;
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Exceptions;
    using MAS.Payments.Infrastructure.Specification;
    using MAS.Payments.Queries.Measurements;

    internal class UpdateMeterMeasurementCommandHandler : BaseCommandHandler<UpdateMeterMeasurementCommand>
    {
        private IRepository<MeterMeasurement> Repository { get; }

        private IRepository<MeterMeasurementType> MeterMeasurementTypeRepository { get; }

        public UpdateMeterMeasurementCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<MeterMeasurement>();
            MeterMeasurementTypeRepository = GetRepository<MeterMeasurementType>();
        }

        public override void Handle(UpdateMeterMeasurementCommand command)
        {
            var measurement = Repository.Get(command.Id);

            Validate(measurement, command);

            var isMeasurementChanged = measurement.Measurement != command.Measurement;
            var isMonthChanged = !measurement.Date.Date.Equals(command.Date);

            var oldMonthDate = new DateTime(measurement.Date.Date.Ticks, DateTimeKind.Utc);
            var oldTypeId = measurement.MeterMeasurementTypeId + 0;

            if (isMonthChanged)
            {
                var itemWithSameType =
                    Repository.Where(new CommonSpecification<MeterMeasurement>(x =>
                        x.MeterMeasurementTypeId == command.MeterMeasurementTypeId
                        && x.Date.Date == command.Date))
                    .FirstOrDefault();

                if (itemWithSameType != null)
                {
                    throw new CommandExecutionException(CommandType, $"Measurement with type \"{itemWithSameType.MeasurementType.Name}\" is already defined for \"{command.Date:MMMMM yyyy}\"");
                }
            }

            Repository.Update(command.Id, new
            {
                command.Comment,
                command.Date,
                command.Measurement,
                command.MeterMeasurementTypeId,
            });

            if (isMonthChanged)
            {
                var previousNextMonthDate = oldMonthDate.AddMonths(1);
                var previousNextMonthItem =
                    Repository.Where(new CommonSpecification<MeterMeasurement>(x =>
                        x.Date.Date.Year == previousNextMonthDate.Year
                        && x.Date.Date.Month == previousNextMonthDate.Month
                        && x.MeterMeasurementTypeId == oldTypeId))
                        .FirstOrDefault();

                if (previousNextMonthItem != null)
                {
                    previousNextMonthItem.Diff = null;
                }
            }

            if (isMeasurementChanged || isMonthChanged)
            {
                var nextMeasurementDate = command.Date.AddMonths(1);
                var nextMeasurementItem =
                    Repository.Where(new CommonSpecification<MeterMeasurement>(x =>
                        x.Date.Date.Year == nextMeasurementDate.Year
                        && x.Date.Date.Month == nextMeasurementDate.Month
                        && x.MeterMeasurementTypeId == command.MeterMeasurementTypeId))
                        .FirstOrDefault();

                if (nextMeasurementItem != null)
                {
                    nextMeasurementItem.Diff = Math.Abs(nextMeasurementItem.Measurement - command.Measurement);
                }
            }
        }

        private void Validate(MeterMeasurement measurement, UpdateMeterMeasurementCommand command)
        {
            if (measurement == null)
            {
                throw new EntityNotFoundException(typeof(MeterMeasurement), command.Id);
            }

            if (command.Measurement <= 0)
            {
                throw new ArgumentException($"Value must be greater than 0.");
            }

            _ =
                MeterMeasurementTypeRepository.Get(command.MeterMeasurementTypeId)
                ?? throw new EntityNotFoundException(typeof(MeterMeasurementType), command.MeterMeasurementTypeId);

            var measurementOnSpecifiedMonth =
                Repository.Where(new CommonSpecification<MeterMeasurement>(x =>
                    x.Date.Date == command.Date
                    && x.MeterMeasurementTypeId == command.MeterMeasurementTypeId
                    && x.Id != command.Id))
                .FirstOrDefault();

            if (measurementOnSpecifiedMonth != null)
            {
                throw new ArgumentException($"Measurement record for {measurementOnSpecifiedMonth.MeasurementType.Name} {command.Date:MMMM yyyy} is already exist.");
            }

            var previousTypeValue =
                    QueryProcessor.Execute(new GetSiblingMeasurementQuery(command.MeterMeasurementTypeId, command.Date, GetSiblingMeasurementDirection.Previous));

            if (previousTypeValue != null && command.Measurement < previousTypeValue.Measurement)
            {
                throw new ArgumentException($"Measurement value \"{command.Measurement}\" must be greater than previous \"{previousTypeValue.Measurement}\".");
            }

             var closestNextMeasurement =
                    QueryProcessor.Execute(new GetSiblingMeasurementQuery(command.MeterMeasurementTypeId, command.Date, GetSiblingMeasurementDirection.Next));

            if (closestNextMeasurement != null && command.Measurement >= closestNextMeasurement.Measurement)
            {
                throw new ArgumentException($"Measurement value \"{command.Measurement}\" must be less than next \"{closestNextMeasurement.Measurement}\".");
            }
        }
    }
}