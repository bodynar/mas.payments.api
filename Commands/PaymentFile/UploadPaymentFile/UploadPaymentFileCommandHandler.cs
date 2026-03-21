namespace MAS.Payments.Commands
{
    using System;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Exceptions;
    using MAS.Payments.Infrastructure.Specification;

    using Microsoft.EntityFrameworkCore;

    internal class UploadPaymentFileCommandHandler : BaseCommandHandler<UploadPaymentFileCommand>
    {
        private IRepository<PaymentFile> Repository { get; }

        private IRepository<Payment> PaymentRepository { get; }

        private IRepository<PaymentGroup> PaymentGroupRepository { get; }

        public UploadPaymentFileCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<PaymentFile>();
            PaymentRepository = GetRepository<Payment>();
            PaymentGroupRepository = GetRepository<PaymentGroup>();
        }

        public override async Task HandleAsync(UploadPaymentFileCommand command)
        {
            if (command.PaymentId.HasValue == command.PaymentGroupId.HasValue)
            {
                throw new CommandExecutionException(CommandType,
                    "File must be linked to either Payment or PaymentGroup, but not both.");
            }

            if (command.PaymentId.HasValue)
            {
                _ = await PaymentRepository.Get(command.PaymentId.Value)
                    ?? throw new CommandExecutionException(CommandType,
                        $"Payment with id {command.PaymentId.Value} doesn't exist.");

                var hasFile = await Repository.Any(
                    new CommonSpecification<PaymentFile>(f => f.PaymentId == command.PaymentId.Value));

                if (hasFile)
                {
                    throw new CommandExecutionException(CommandType,
                        $"Payment with id {command.PaymentId.Value} already has an attached file.");
                }
            }

            if (command.PaymentGroupId.HasValue)
            {
                _ = await PaymentGroupRepository.Get(command.PaymentGroupId.Value)
                    ?? throw new CommandExecutionException(CommandType,
                        $"PaymentGroup with id {command.PaymentGroupId.Value} doesn't exist.");

                var hasFile = await Repository.Any(
                    new CommonSpecification<PaymentFile>(f => f.PaymentGroupId == command.PaymentGroupId.Value));

                if (hasFile)
                {
                    throw new CommandExecutionException(CommandType,
                        $"PaymentGroup with id {command.PaymentGroupId.Value} already has an attached file.");
                }
            }

            var file = new PaymentFile
            {
                FileName = command.FileName,
                FileSize = command.Data.Length,
                ContentType = command.ContentType,
                Data = command.Data,
                PaymentId = command.PaymentId,
                PaymentGroupId = command.PaymentGroupId,
            };

            await Repository.Add(file);
        }
    }
}
