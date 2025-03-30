namespace MAS.Payments.Commands.Common.CreatePdfDocument
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;

    internal class CreatePdfDocumentCommandHandler : BaseCommandHandler<CreatePdfDocumentCommand>
    {
        private IRepository<PdfDocument> Repository { get; }

        public CreatePdfDocumentCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<PdfDocument>();
        }

        public override async Task HandleAsync(CreatePdfDocumentCommand command)
        {
            ArgumentNullException.ThrowIfNull(command.File);

            if (command.File.Length == 0)
            {
                return;
            }

            using var memoryStream = new MemoryStream();
            await command.File.CopyToAsync(memoryStream);

            var document = new PdfDocument
            {
                Name = command.File.FileName,
                FileData = memoryStream.ToArray()
            };

            await Repository.Add(document);

            command.PdfDocument = document;
        }
    }
}
