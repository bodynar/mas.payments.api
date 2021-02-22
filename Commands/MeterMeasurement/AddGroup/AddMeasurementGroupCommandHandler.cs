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

            // TODO: validate measurement

            var notValidTypes =
                command.Measurements
                    .Where(x => MeterMeasurementTypeRepository.Get(x.MeasurementTypeId) == null)
                    .Select(x => x.MeasurementTypeId);

            if (notValidTypes.Any())
            {
                throw new CommandExecutionException(CommandType, $"Measurement types with ids [{string.Join(",", notValidTypes)}] doesn't exists");
            }

            var measurements = new List<MeterMeasurement>();
            var calculatedNewMeasurementDate = new DateTime(command.Date.Year, command.Date.Month, 20);
            var previousMonthDate = calculatedNewMeasurementDate.AddMonths(-1);

            var measurementsOnSpecifiedMonth =
                Repository.Where(new CommonSpecification<MeterMeasurement>(x => x.Date.Date == calculatedNewMeasurementDate));

            foreach (var measurementData in command.Measurements)
            {
                var measurementWithSameType =
                    measurementsOnSpecifiedMonth.FirstOrDefault(x => x.MeterMeasurementTypeId == measurementData.MeasurementTypeId);

                if (measurementWithSameType != null)
                {
                    throw new ArgumentException($"Measurement for date {calculatedNewMeasurementDate:MMMM yyyy} with type \"{measurementWithSameType.MeasurementType.Name}\" is already added.");
                }

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
    }
}
