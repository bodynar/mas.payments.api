namespace MAS.Payments.Commands
{
    using System.Linq;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Exceptions;
    using MAS.Payments.Infrastructure.Specification;

    using Microsoft.EntityFrameworkCore;

    internal class DeletePaymentGroupCommandHandler : BaseCommandHandler<DeletePaymentGroupCommand>
    {
        private IRepository<DataBase.PaymentGroup> PaymentGroupRepository { get; }

        private IRepository<Payment> PaymentRepository { get; }

        public DeletePaymentGroupCommandHandler(IResolver resolver)
            : base(resolver)
        {
            PaymentGroupRepository = GetRepository<DataBase.PaymentGroup>();
            PaymentRepository = GetRepository<Payment>();
        }

        public override async Task HandleAsync(DeletePaymentGroupCommand command)
        {
            _ = await PaymentGroupRepository.Get(command.PaymentGroupId)
                ?? throw new CommandExecutionException(CommandType,
                    $"Payment group with id {command.PaymentGroupId} doesn't exist");

            var linkedPayments =
                await PaymentRepository
                    .Where(new CommonSpecification<Payment>(x => x.PaymentGroupId == command.PaymentGroupId))
                    .ToListAsync();

            await PaymentRepository.DeleteRange(linkedPayments);

            await PaymentGroupRepository.Delete(command.PaymentGroupId);
        }
    }
}
