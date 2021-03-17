namespace MAS.Payments.Commands
{
    using System;
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Specification;

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
            var deletedItem = Repository.Get(command.MeterMeasurementId);

            if (deletedItem == null)
            {
                throw new ArgumentException($"Measurement with id \"{command.MeterMeasurementId}\" doen't exists");
            }

            var nextMeasurementDate = deletedItem.Date.Date.AddMonths(1);

            Repository.Delete(command.MeterMeasurementId);

            var nextMeasurementItem = Repository.Where(new CommonSpecification<MeterMeasurement>(x => x.Date == nextMeasurementDate)).FirstOrDefault();

            if (nextMeasurementItem != null)
            {
                nextMeasurementItem.Diff = null;
            }
        }
    }
}