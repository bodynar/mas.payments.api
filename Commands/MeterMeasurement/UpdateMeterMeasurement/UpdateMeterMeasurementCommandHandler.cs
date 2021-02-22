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
            // TODO: validate measurement
            var meterMeasurementType =
                MeterMeasurementTypeRepository.Get(command.MeterMeasurementTypeId);

            if (meterMeasurementType == null)
            {
                throw new CommandExecutionException(CommandType,
                    $"Measurement type with id {command.MeterMeasurementTypeId} doesn't exist");
            }

            var measurement = Repository.Get(command.Id);

            if (measurement == null)
            {
                throw new CommandExecutionException(CommandType, $"Measurement with id {command.Id} doesn't exist");
            }

            var newDate = new DateTime(command.Date.Year, command.Date.Month, 20);

            var isMeasurementChanged = measurement.Measurement != command.Measurement;
            var isMonthChanged = !measurement.Date.Date.Equals(newDate);

            var oldMonthDate = new DateTime(measurement.Date.Date.Ticks);
            var oldTypeId = measurement.MeterMeasurementTypeId + 0;

            if (isMonthChanged)
            {
                var itemWithSameType =
                    Repository.Where(new CommonSpecification<MeterMeasurement>(x =>
                        x.MeterMeasurementTypeId == command.MeterMeasurementTypeId
                        && x.Date.Date == newDate))
                    .FirstOrDefault();

                if (itemWithSameType != null)
                {
                    throw new CommandExecutionException(CommandType, $"Measurement with type \"{itemWithSameType.MeasurementType.Name}\" is already defined for \"{newDate:MMMMM yyyy}\"");
                }
            }

            Repository.Update(command.Id, new
            {
                Comment = command.Comment,
                Date = newDate,
                Measurement = command.Measurement,
                MeterMeasurementTypeId = command.MeterMeasurementTypeId,
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
                var nextMeasurementDate = newDate.AddMonths(1);
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
    }
}