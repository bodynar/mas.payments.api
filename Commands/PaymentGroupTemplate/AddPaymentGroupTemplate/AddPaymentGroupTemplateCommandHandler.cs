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

    internal class AddPaymentGroupTemplateCommandHandler : BaseCommandHandler<AddPaymentGroupTemplateCommand>
    {
        private IRepository<PaymentGroupTemplate> TemplateRepository { get; }

        private IRepository<PaymentType> PaymentTypeRepository { get; }

        public AddPaymentGroupTemplateCommandHandler(IResolver resolver)
            : base(resolver)
        {
            TemplateRepository = GetRepository<PaymentGroupTemplate>();
            PaymentTypeRepository = GetRepository<PaymentType>();
        }

        public override async Task HandleAsync(AddPaymentGroupTemplateCommand command)
        {
            var alreadyExists =
                await TemplateRepository.GetAll().Any(
                    new CommonSpecification<PaymentGroupTemplate>(x => x.Name == command.Name));

            if (alreadyExists)
            {
                throw new CommandExecutionException(CommandType,
                    $"Payment group Template with name \"{command.Name}\" already exists");
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
                    $"Payment types with ids [{string.Join(", ", missingIds)}] don't exist");
            }

            var template = new PaymentGroupTemplate
            {
                Name = command.Name,
                Description = command.Description,
            };

            foreach (var paymentTypeId in paymentTypeIds)
            {
                template.Items.Add(new PaymentGroupTemplateItem
                {
                    PaymentTypeId = paymentTypeId,
                });
            }

            await TemplateRepository.Add(template);
        }
    }
}
