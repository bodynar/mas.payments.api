namespace MAS.Payments.Commands
{
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Exceptions;

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

            var measurements =
                command.Measurements
                    .Select(x => new MeterMeasurement
                    {
                        Date = command.Date,
                        Measurement = x.Measurement,
                        MeterMeasurementTypeId = x.MeasurementTypeId,
                        Comment = x.Comment,
                    });

            Repository.AddRange(measurements);
        }
    }
}
