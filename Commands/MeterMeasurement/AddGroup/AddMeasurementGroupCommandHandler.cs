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
            var notValidTypes =
                command.Measurements
                    .Where(x => MeterMeasurementTypeRepository.Get(x.MeasurementTypeId) == null)
                    .Select(x => x.MeasurementTypeId);

            if (notValidTypes.Any())
            {
                throw new CommandExecutionException(CommandType, $"Measurement types with ids [{string.Join(",", notValidTypes)}] doesn't exists");
            }

            var requestDate = command.Date.Date.AddMonths(-1);
            var measurements = new List<MeterMeasurement>();

            foreach (var measurementData in command.Measurements)
            {
                var previousValue =
                    Repository.Where(new CommonSpecification<MeterMeasurement>(x => x.Date.Date == requestDate))
                        .FirstOrDefault();

                double? diff = null;

                if (previousValue != null)
                {
                    diff = Math.Abs(previousValue.Measurement - measurementData.Measurement);
                }

                measurements.Add(
                    new MeterMeasurement
                    {
                        Date = command.Date.Date,
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
