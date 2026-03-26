namespace MAS.Payments.Commands
{
    using System.Linq;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Exceptions;
    using MAS.Payments.Infrastructure.Extensions;
    using MAS.Payments.Infrastructure.Specification;

    using Microsoft.EntityFrameworkCore;

    internal class UpdatePaymentGroupTemplateCommandHandler : BaseCommandHandler<UpdatePaymentGroupTemplateCommand>
    {
        private IRepository<PaymentGroupTemplate> TemplateRepository { get; }

        private IRepository<PaymentGroupTemplateItem> TemplateItemRepository { get; }

        private IRepository<PaymentType> PaymentTypeRepository { get; }

        public UpdatePaymentGroupTemplateCommandHandler(IResolver resolver)
            : base(resolver)
        {
            TemplateRepository = GetRepository<PaymentGroupTemplate>();
            TemplateItemRepository = GetRepository<PaymentGroupTemplateItem>();
            PaymentTypeRepository = GetRepository<PaymentType>();
        }

        public override async Task HandleAsync(UpdatePaymentGroupTemplateCommand command)
        {
            var isNotUnique =
                await TemplateRepository.Any(
                    new CommonSpecification<PaymentGroupTemplate>(
                        x => x.Id != command.Id && x.Name == command.Name));

            if (isNotUnique)
            {
                throw new CommandExecutionException(CommandType,
                    $"Payment group template with name \"{command.Name}\" already exists");
            }

            var paymentTypeIds = command.PaymentTypeIds.ToArray();

            var existingTypeIds =
                await PaymentTypeRepository
                    .Where(new CommonSpecification.IdIn<PaymentType>(paymentTypeIds))
                    .Select(x => x.Id)
                    .ToArrayAsync();

            var missingIds = paymentTypeIds.Except(existingTypeIds);

            if (missingIds.Any())
            {
                throw new CommandExecutionException(CommandType,
                    $"Payment types with ids [{string.Join(", ", missingIds)}] do not exist");
            }

            await TemplateRepository.Update(command.Id, new
            {
                command.Name,
                command.Description,
            });

            var existingItems =
                await TemplateItemRepository
                    .Where(new CommonSpecification<PaymentGroupTemplateItem>(
                        x => x.PaymentGroupTemplateId == command.Id))
                    .ToListAsync();

            await TemplateItemRepository.DeleteRange(existingItems);

            foreach (var paymentTypeId in paymentTypeIds)
            {
                await TemplateItemRepository.Add(new PaymentGroupTemplateItem
                {
                    PaymentGroupTemplateId = command.Id,
                    PaymentTypeId = paymentTypeId,
                });
            }
        }
    }
}
