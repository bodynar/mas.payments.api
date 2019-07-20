using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.Exceptions;
using MAS.Payments.Infrastructure.Extensions;
using MAS.Payments.Infrastructure.Specification;

namespace MAS.Payments.Commands
{
    internal class AddMeterMeasurementTypeCommandHandler : BaseCommandHandler<AddMeterMeasurementTypeCommand>
    {
        private IRepository<MeterMeasurementType> Repository { get; }

        public AddMeterMeasurementTypeCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<MeterMeasurementType>();
        }

        public override void Handle(AddMeterMeasurementTypeCommand command)
        {
            var isUnique =
                !Repository.GetAll().Any(
                    new CommonSpecification<MeterMeasurementType>(x => 
                        x.Name == command.Name && x.PaymentTypeId == command.PaymentTypeId));

            if (!isUnique)
            {
                throw new CommandExecutionException(
                    $"Measurement type with name {command.Name} is already exist");
            }

            var paymentType =
                GetRepository<PaymentType>().Get(command.PaymentTypeId);

            if (paymentType == null)
            {
                throw new CommandExecutionException(
                    $"Payment type with id {command.PaymentTypeId} doesn't exist");
            }

            var meterMeasurementType = new MeterMeasurementType
            {
                Name = command.Name,
                Description = command.Description,
                PaymentTypeId = command.PaymentTypeId
            };

            Repository.Add(meterMeasurementType);
        }
    }
}