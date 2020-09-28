namespace MAS.Payments.Commands
{
    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;

    internal class DeletePaymentCommandHandler : BaseCommandHandler<DeletePaymentCommand>
    {
        private IRepository<Payment> Repository { get; }

        public DeletePaymentCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<Payment>();
        }

        public override void Handle(DeletePaymentCommand command)
        {
            Repository.Delete(command.PaymentId);
        }
    }
}