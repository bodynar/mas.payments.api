using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    internal class DeleteMeterMeasurementTypeCommandHandler : BaseCommandHandler<DeleteMeterMeasurementTypeCommand>
    {
        private IRepository<MeterMeasurementType> Repository { get; }

        public DeleteMeterMeasurementTypeCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<MeterMeasurementType>();
        }

        public override void Handle(DeleteMeterMeasurementTypeCommand command)
        {
            Repository.Delete(command.MeterMeasurementTypeId);
        }
    }
}