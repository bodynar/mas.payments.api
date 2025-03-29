namespace MAS.Payments.Commands
{
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Exceptions;
    using MAS.Payments.Infrastructure.Specification;

    internal class AddMeterMeasurementTypeCommandHandler : BaseCommandHandler<AddMeterMeasurementTypeCommand>
    {
        private IRepository<MeterMeasurementType> Repository { get; }

        private IRepository<PaymentType> PaymentTypeRepository { get; }

        public AddMeterMeasurementTypeCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<MeterMeasurementType>();
            PaymentTypeRepository = GetRepository<PaymentType>();
        }

        public override async Task HandleAsync(AddMeterMeasurementTypeCommand command)
        {
            var isNotUnique =
                await Repository.Any(
                    new CommonSpecification<MeterMeasurementType>(x =>
                        x.Name == command.Name && x.PaymentTypeId == command.PaymentTypeId));

            if (isNotUnique)
            {
                throw new CommandExecutionException(CommandType,
                    $"Measurement type with name {command.Name} is already exist");
            }

            _ =
                await PaymentTypeRepository.Get(command.PaymentTypeId)
                ?? throw new CommandExecutionException(CommandType, $"Payment type with id {command.PaymentTypeId} doesn't exist");

            var meterMeasurementType = new MeterMeasurementType
            {
                Name = command.Name,
                Description = command.Description,
                PaymentTypeId = command.PaymentTypeId,
                Color = command.Color,
            };

            await Repository.Add(meterMeasurementType);
        }
    }
}