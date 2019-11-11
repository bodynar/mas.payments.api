using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.Exceptions;

namespace MAS.Payments.Commands
{
    internal class AddMeterMeasurementCommandHandler : BaseCommandHandler<AddMeterMeasurementCommand>
    {
        private IRepository<MeterMeasurement> Repository { get; }

        private IRepository<MeterMeasurementType> MeterMeasurementTypeRepository { get; }

        public AddMeterMeasurementCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<MeterMeasurement>();
            MeterMeasurementTypeRepository = GetRepository<MeterMeasurementType>();
        }

        public override void Handle(AddMeterMeasurementCommand command)
        {
            var meterMeasurementType =
                MeterMeasurementTypeRepository.Get(command.MeterMeasurementTypeId);

            if (meterMeasurementType == null)
            {
                throw new CommandExecutionException(
                    $"Measurement type with id {command.MeterMeasurementTypeId} doesn't exist");
            }

            var meterMeasurement = new MeterMeasurement
            {
                Measurement = command.Measurement,
                Comment = command.Comment,
                Date = command.Date,
                MeterMeasurementTypeId = command.MeterMeasurementTypeId,
                AuthorId = command.UserId
            };

            Repository.Add(meterMeasurement);
        }
    }
}