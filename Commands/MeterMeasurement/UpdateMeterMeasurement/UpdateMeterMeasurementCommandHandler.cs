namespace MAS.Payments.Commands
{
    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Exceptions;

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
            var meterMeasurementType =
                MeterMeasurementTypeRepository.Get(command.MeterMeasurementTypeId);

            if (meterMeasurementType == null)
            {
                throw new CommandExecutionException(CommandType,
                    $"Measurement type with id {command.MeterMeasurementTypeId} doesn't exist");
            }

            Repository.Update(command.Id, new
            {
                Comment = command.Comment,
                Date = command.Date,
                Measurement = command.Measurement,
                MeterMeasurementTypeId = command.MeterMeasurementTypeId,
            });
        }
    }
}