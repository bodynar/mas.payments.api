namespace MAS.Payments.Commands
{
    using System;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Exceptions;
    using MAS.Payments.Infrastructure.Specification;
    using MAS.Payments.Queries.Measurements;

    using Microsoft.EntityFrameworkCore;

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

        public override async Task HandleAsync(UpdateMeterMeasurementCommand command)
        {
            var measurement = await Repository.Get(command.Id);

            await ValidateAsync(measurement, command);

            var isMeasurementChanged = measurement.Measurement != command.Measurement;
            var isMonthChanged = !measurement.Date.Date.Equals(command.Date);

            var oldMonthDate = new DateTime(measurement.Date.Date.Ticks, DateTimeKind.Utc);
            var oldTypeId = measurement.MeterMeasurementTypeId + 0;

            if (isMonthChanged)
            {
                var itemWithSameType =
                    await Repository.Where(new CommonSpecification<MeterMeasurement>(x =>
                        x.MeterMeasurementTypeId == command.MeterMeasurementTypeId
                        && x.Date.Date == command.Date))
                    .FirstOrDefaultAsync();

                if (itemWithSameType != null)
                {
                    throw new CommandExecutionException(CommandType, $"Measurement with type \"{itemWithSameType.MeasurementType.Name}\" is already defined for \"{command.Date:MMMMM yyyy}\"");
                }
            }

            await Repository.Update(command.Id, new
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
                    await Repository.Where(new CommonSpecification<MeterMeasurement>(x =>
                        x.Date.Date.Year == previousNextMonthDate.Year
                        && x.Date.Date.Month == previousNextMonthDate.Month
                        && x.MeterMeasurementTypeId == oldTypeId))
                        .FirstOrDefaultAsync();

                if (previousNextMonthItem != null)
                {
                    previousNextMonthItem.Diff = null;
                }
            }

            if (isMeasurementChanged || isMonthChanged)
            {
                var nextMeasurementDate = command.Date.AddMonths(1);
                var nextMeasurementItem =
                    await Repository.Where(new CommonSpecification<MeterMeasurement>(x =>
                        x.Date.Date.Year == nextMeasurementDate.Year
                        && x.Date.Date.Month == nextMeasurementDate.Month
                        && x.MeterMeasurementTypeId == command.MeterMeasurementTypeId)
                    )
                        .FirstOrDefaultAsync();

                if (nextMeasurementItem != null)
                {
                    nextMeasurementItem.Diff = Math.Abs(nextMeasurementItem.Measurement - command.Measurement);
                }
            }
        }

        private async Task ValidateAsync(MeterMeasurement measurement, UpdateMeterMeasurementCommand command)
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
                await Repository.Where(new CommonSpecification<MeterMeasurement>(x =>
                    x.Date.Date == command.Date
                    && x.MeterMeasurementTypeId == command.MeterMeasurementTypeId
                    && x.Id != command.Id))
                .FirstOrDefaultAsync();

            if (measurementOnSpecifiedMonth != null)
            {
                throw new ArgumentException($"Measurement record for {measurementOnSpecifiedMonth.MeasurementType.Name} {command.Date:MMMM yyyy} is already exist.");
            }

            var previousTypeValue =
                    await QueryProcessor.Execute(new GetSiblingMeasurementQuery(command.MeterMeasurementTypeId, command.Date, GetSiblingMeasurementDirection.Previous));

            if (previousTypeValue != null && command.Measurement < previousTypeValue.Measurement)
            {
                throw new ArgumentException($"Measurement value \"{command.Measurement}\" must be greater than previous \"{previousTypeValue.Measurement}\".");
            }

            var closestNextMeasurement =
                  await QueryProcessor.Execute(new GetSiblingMeasurementQuery(command.MeterMeasurementTypeId, command.Date, GetSiblingMeasurementDirection.Next));

            if (closestNextMeasurement != null && command.Measurement >= closestNextMeasurement.Measurement)
            {
                throw new ArgumentException($"Measurement value \"{command.Measurement}\" must be less than next \"{closestNextMeasurement.Measurement}\".");
            }
        }
    }
}