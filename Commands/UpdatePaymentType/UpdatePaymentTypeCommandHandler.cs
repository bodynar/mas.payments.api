using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.Exceptions;
using MAS.Payments.Infrastructure.Extensions;
using MAS.Payments.Infrastructure.Specification;

namespace MAS.Payments.Commands
{
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
                throw new CommandExecutionException(
                    $"Payment type with name {command.Name} from {command.Company} is already exist");
            }

            var updatedPaymenentyType = Repository.Get(command.Id);

            updatedPaymenentyType.Company = command.Company;
            updatedPaymenentyType.Description = command.Description;
            updatedPaymenentyType.Name = command.Name;

            Repository.Update(command.Id, updatedPaymenentyType);
        }
    }
}