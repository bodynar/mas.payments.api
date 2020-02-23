using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    internal class DeletePaymentTypeCommandHandler : BaseCommandHandler<DeletePaymentTypeCommand>
    {
        private IRepository<PaymentType> Repository { get; }

        public DeletePaymentTypeCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<PaymentType>();
        }

        public override void Handle(DeletePaymentTypeCommand command)
        {
            Repository.Delete(command.PaymentTypeId);
        }
    }
}