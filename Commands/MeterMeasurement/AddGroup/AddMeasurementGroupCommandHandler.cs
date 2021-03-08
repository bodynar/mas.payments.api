namespace MAS.Payments.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Exceptions;
    using MAS.Payments.Infrastructure.Specification;

    public class AddMeasurementGroupCommandHandler : BaseCommandHandler<AddMeasurementGroupCommand>
    {
        private IRepository<MeterMeasurement> Repository { get; }

        private IRepository<MeterMeasurementType> MeterMeasurementTypeRepository { get; }

        public AddMeasurementGroupCommandHandler(IResolver resolver)
            : base(resolver)
        {
            Repository = GetRepository<MeterMeasurement>();
            MeterMeasurementTypeRepository = GetRepository<MeterMeasurementType>();
        }

        public override void Handle(AddMeasurementGroupCommand command)
        {
            if (!command.Measurements.Any())
            {
                return;
            }

            Validate(command);

            var measurements = new List<MeterMeasurement>();
            var calculatedNewMeasurementDate = new DateTime(command.Date.Year, command.Date.Month, 20);
            var previousMonthDate = calculatedNewMeasurementDate.AddMonths(-1);

            foreach (var measurementData in command.Measurements)
            {
                var previousValue =
                    Repository.Where(new CommonSpecification<MeterMeasurement>(x =>
                        x.Date.Date.Month == previousMonthDate.Month
                        && x.Date.Date.Year == previousMonthDate.Year
                        && x.MeterMeasurementTypeId == measurementData.MeasurementTypeId
                    )).FirstOrDefault();

                double? diff = null;

                if (previousValue != null)
                {
                    diff = Math.Abs(previousValue.Measurement - measurementData.Measurement);
                }

                measurements.Add(
                    new MeterMeasurement
                    {
                        Date = calculatedNewMeasurementDate,
                        Measurement = measurementData.Measurement,
                        MeterMeasurementTypeId = measurementData.MeasurementTypeId,
                        Comment = measurementData.Comment,
                        Diff = diff
                    });
            }

            Repository.AddRange(measurements);
        }

        private void Validate(AddMeasurementGroupCommand command)
        {
            var notValidMeasurements = command.Measurements.Where(x => x.Measurement <= 0).Select(x => x.Measurement);

            if (notValidMeasurements.Any())
            {
                throw new ArgumentException($"Cannot add measurements with value [{string.Join(", ", notValidMeasurements)}]. Value must be greater than 0.");
            }

            var hasDuplicateTypes =
                new HashSet<long>(command.Measurements.Select(x => x.MeasurementTypeId)).Count != command.Measurements.Count();

            if (hasDuplicateTypes)
            {
                throw new ArgumentException("Cannot add measurements with duplicate types.");
            }

            var notValidTypes =
                command.Measurements
                    .Where(x => MeterMeasurementTypeRepository.Get(x.MeasurementTypeId) == null)
                    .Select(x => x.MeasurementTypeId);

            if (notValidTypes.Any())
            {
                throw new CommandExecutionException(CommandType, $"Measurement types with ids [{string.Join(",", notValidTypes)}] doesn't exist.");
            }

            var calculatedNewMeasurementDate = new DateTime(command.Date.Year, command.Date.Month, 20);

            var measurementsOnSpecifiedMonth =
                Repository.Where(new CommonSpecification<MeterMeasurement>(x => x.Date.Date == calculatedNewMeasurementDate))
                .ToList();

            var duplicateTypes =
                command.Measurements
                    .Where(measurementData =>
                        measurementsOnSpecifiedMonth.FirstOrDefault(x => x.MeterMeasurementTypeId == measurementData.MeasurementTypeId) != null)
                    .Select(x => new {
                        TypeName = MeterMeasurementTypeRepository.Get(x.MeasurementTypeId).Name,
                        x.Measurement
                    }).Select(x => $"{x.TypeName} - {x.Measurement}");

            if (duplicateTypes.Any())
            {
                throw new ArgumentException($"Cannot add duplicate measurements for types which already exist for {calculatedNewMeasurementDate:MMMM yyyy}: {string.Join("; ", duplicateTypes)}.");
            }

            foreach (var measurementData in command.Measurements)
            {
                var previousTypeValue =
                    Repository.Where(new CommonSpecification<MeterMeasurement>(x => x.MeterMeasurementTypeId == measurementData.MeasurementTypeId))
                    .OrderByDescending(x => x.Date.Date)
                    .FirstOrDefault();

                if (measurementData.Measurement < previousTypeValue.Measurement)
                {
                    throw new ArgumentException($"Measurement value must be greater than previous. Cannot add value \"{measurementData.Measurement}\" that is less than previous \"{previousTypeValue.Measurement}\".");
                }
            }
        }
    }
}
