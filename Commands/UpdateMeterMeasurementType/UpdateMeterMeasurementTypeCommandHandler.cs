using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.Exceptions;

namespace MAS.Payments.Commands
{
    internal class UpdateMeterMeasurementTypeCommandHandler : BaseCommandHandler<UpdateMeterMeasurementTypeCommand>
    {
        private IRepository<MeterMeasurementType> Repository { get; }

        private IRepository<PaymentType> PaymentTypeRepository { get; }

        public UpdateMeterMeasurementTypeCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<MeterMeasurementType>();
            PaymentTypeRepository = GetRepository<PaymentType>();
        }

        public override void Handle(UpdateMeterMeasurementTypeCommand command)
        {
            var paymentType =
                PaymentTypeRepository.Get(command.PaymentTypeId);

            if (paymentType == null)
            {
                throw new CommandExecutionException(
                    $"Payment type with id {command.PaymentTypeId} doesn't exist");
            }

            var updatedEntity = Repository.Get(command.Id);

            updatedEntity.Name = command.Name;
            updatedEntity.PaymentTypeId = command.PaymentTypeId;
            updatedEntity.Description = command.Description;

            Repository.Update(command.Id, updatedEntity);
        }
    }
}