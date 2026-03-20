namespace MAS.Payments.Commands
{
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Exceptions;
    using MAS.Payments.Infrastructure.Specification;

    using Microsoft.EntityFrameworkCore;

    internal class DeleteMeterMeasurementCommandHandler : BaseCommandHandler<DeleteMeterMeasurementCommand>
    {
        private IRepository<MeterMeasurement> Repository { get; }

        public DeleteMeterMeasurementCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<MeterMeasurement>();
        }

        public override async Task HandleAsync(DeleteMeterMeasurementCommand command)
        {
            var deletedItem = await Repository.Get(command.MeterMeasurementId)
                ?? throw new EntityNotFoundException(typeof(MeterMeasurement), command.MeterMeasurementId);

            var nextMeasurementDate = deletedItem.Date.Date.AddMonths(1);

            await Repository.Delete(command.MeterMeasurementId);

            var nextMeasurementItem = await Repository
                .Where(new CommonSpecification<MeterMeasurement>(x => x.Date == nextMeasurementDate))
                .FirstOrDefaultAsync();

            if (nextMeasurementItem != null)
            {
                nextMeasurementItem.Diff = null;
            }
        }
    }
}