using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    internal class DeleteMeterMeasurementCommandHandler : BaseCommandHandler<DeleteMeterMeasurementCommand>
    {
        private IRepository<MeterMeasurement> Repository { get; }

        public DeleteMeterMeasurementCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<MeterMeasurement>();
        }

        public override void Handle(DeleteMeterMeasurementCommand command)
        {
            Repository.Delete(command.MeterMeasurementId);
        }
    }
}