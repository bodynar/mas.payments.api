namespace MAS.Payments.Commands
{
    using MAS.Payments.DataBase;
    using MAS.Payments.Infrastructure.Command;

    using Microsoft.AspNetCore.Http;

    public class CreatePdfDocumentCommand(
        IFormFile file
    ) : ICommand
    {
        public IFormFile File { get; } = file;

        public PdfDocument PdfDocument { get; set; }
    }
}
