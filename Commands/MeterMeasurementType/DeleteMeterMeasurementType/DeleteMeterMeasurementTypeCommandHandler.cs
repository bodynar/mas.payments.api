namespace MAS.Payments.Commands
{
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;

    internal class DeleteMeterMeasurementTypeCommandHandler : BaseCommandHandler<DeleteMeterMeasurementTypeCommand>
    {
        private IRepository<MeterMeasurementType> Repository { get; }

        public DeleteMeterMeasurementTypeCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<MeterMeasurementType>();
        }

        public override async Task HandleAsync(DeleteMeterMeasurementTypeCommand command)
        {
            await Repository.Delete(command.MeterMeasurementTypeId);
        }
    }
}