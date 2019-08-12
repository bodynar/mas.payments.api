using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.Exceptions;

namespace MAS.Payments.Commands
{
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
                throw new CommandExecutionException(
                    $"Measurement type with id {command.MeterMeasurementTypeId} doesn't exist");
            }

            var updatedEntity = Repository.Get(command.Id);

            updatedEntity.Comment = command.Comment;
            updatedEntity.Date = command.Date;
            updatedEntity.Measurement = command.Measurement;
            updatedEntity.MeterMeasurementTypeId = command.MeterMeasurementTypeId;

            Repository.Update(command.Id, updatedEntity);
        }
    }
}