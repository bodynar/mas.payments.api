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
                throw new CommandExecutionException(CommandType,
                    $"Payment type with id {command.PaymentTypeId} doesn't exist");
            }

            Repository.Update(command.Id, new
            {
                Name = command.Name,
                PaymentTypeId = command.PaymentTypeId,
                Description = command.Description,
            });
        }
    }
}