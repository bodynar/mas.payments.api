namespace MAS.Payments.Commands
{
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;

    internal class DeletePaymentTypeCommandHandler : BaseCommandHandler<DeletePaymentTypeCommand>
    {
        private IRepository<PaymentType> Repository { get; }

        public DeletePaymentTypeCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<PaymentType>();
        }

        public override async Task HandleAsync(DeletePaymentTypeCommand command)
        {
            await Repository.Delete(command.PaymentTypeId);
        }
    }
}