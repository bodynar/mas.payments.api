namespace MAS.Payments.Commands
{
    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Exceptions;
    using MAS.Payments.Infrastructure.Extensions;
    using MAS.Payments.Infrastructure.Specification;

    internal class UpdatePaymentTypeCommandHandler : BaseCommandHandler<UpdatePaymentTypeCommand>
    {
        private IRepository<PaymentType> Repository { get; }

        public UpdatePaymentTypeCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<PaymentType>();
        }

        public override void Handle(UpdatePaymentTypeCommand command)
        {
            var isUnique =
                !Repository.GetAll().Any(
                    new CommonSpecification<PaymentType>(
                        x => x.Id != command.Id
                        && x.Name == command.Name
                        && x.Company == command.Company));

            if (!isUnique)
            {
                throw new CommandExecutionException(CommandType,
                    $"Payment type with name {command.Name} from {command.Company} is already exist");
            }

            Repository.Update(command.Id, new
            {
                Company = command.Company,
                Description = command.Description,
                Name = command.Name,
                Color = command.Color,
            });
        }
    }
}