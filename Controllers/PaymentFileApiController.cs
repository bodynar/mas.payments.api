namespace MAS.Payments.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using MAS.Payments.Commands;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Queries;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/paymentFile")]
    public class PaymentFileApiController(
        IResolver resolver
    ) : BaseApiController(resolver)
    {
        private static readonly string AllowedContentType = "application/pdf";
        private static readonly byte[] PdfMagicBytes = [0x25, 0x50, 0x44, 0x46]; // %PDF
        private const long MaxFileSizeBytes = 25 * 1024 * 1024; // 25 MB
        private const int MaxFileNameLength = 255;

        [HttpPost("[action]")]
        [Consumes("multipart/form-data")]
        public async Task UploadFileAsync(
            IFormFile file,
            [FromForm] Guid? paymentId,
            [FromForm] Guid? paymentGroupId)
        {
            ArgumentNullException.ThrowIfNull(file);

            if (paymentId.HasValue == paymentGroupId.HasValue)
            {
                throw new ArgumentException("Exactly one of paymentId or paymentGroupId must be provided.");
            }

            if (file.Length > MaxFileSizeBytes)
            {
                throw new ArgumentException($"File size exceeds the maximum allowed size of {MaxFileSizeBytes / (1024 * 1024)} MB.");
            }

            if (!string.Equals(file.ContentType, AllowedContentType, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Only PDF files are allowed.");
            }

            if (!await HasPdfSignatureAsync(file))
            {
                throw new ArgumentException("File content is not a valid PDF.");
            }

            var fileName = SanitizeFileName(file.FileName);

            using var memoryStream = new MemoryStream((int)file.Length);
            await file.CopyToAsync(memoryStream);

            var fileBytes = memoryStream.ToArray();
            await CommandProcessor.Execute(
                new UploadPaymentFileCommand(fileName, AllowedContentType, fileBytes, paymentId, paymentGroupId));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetFileAsync(Guid? id)
        {
            if (!id.HasValue || id.Value == default)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var file = await QueryProcessor.Execute(new GetPaymentFileQuery(id.Value));

            if (file == null)
            {
                return NotFound();
            }

            return File(file.Data, file.ContentType, file.FileName);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ViewFileAsync(Guid? id)
        {
            if (!id.HasValue || id.Value == default)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var file = await QueryProcessor.Execute(new GetPaymentFileQuery(id.Value));

            if (file == null)
            {
                return NotFound();
            }

            return File(file.Data, file.ContentType);
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<GetPaymentFilesResponse>> GetFilesAsync()
        {
            return await QueryProcessor.Execute(new GetPaymentFilesQuery());
        }

        [HttpPost("[action]")]
        public async Task DeleteFileAsync([FromBody] Models.DeleteRecordRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await CommandProcessor.Execute(new DeletePaymentFileCommand(request.Id));
        }

        [HttpPost("[action]")]
        public async Task DeleteFilesAsync([FromBody] Models.DeleteRecordsRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(request.Ids);

            if (!request.Ids.Any())
            {
                throw new ArgumentException("Ids must not be empty.");
            }

            await CommandProcessor.Execute(new DeletePaymentFilesCommand(request.Ids));
        }

        private static string SanitizeFileName(string fileName)
        {
            var name = Path.GetFileName(fileName) ?? "file.pdf";

            var invalidChars = Path.GetInvalidFileNameChars();
            name = string.Concat(name.Where(c => !invalidChars.Contains(c)));

            if (string.IsNullOrWhiteSpace(name))
            {
                name = "file.pdf";
            }

            if (name.Length > MaxFileNameLength)
            {
                var ext = Path.GetExtension(name);

                if (string.IsNullOrEmpty(ext) || ext.Length >= MaxFileNameLength)
                {
                    name = name.Substring(0, MaxFileNameLength);
                }
                else
                {
                    name = string.Concat(name.AsSpan(0, MaxFileNameLength - ext.Length), ext);
                }
            }

            return name;
        }

        private static async Task<bool> HasPdfSignatureAsync(IFormFile file)
        {
            if (file.Length < PdfMagicBytes.Length)
            {
                return false;
            }

            var header = new byte[PdfMagicBytes.Length];
            using var stream = file.OpenReadStream();
            var bytesRead = 0;
            while (bytesRead < header.Length)
            {
                var read = await stream.ReadAsync(header, bytesRead, header.Length - bytesRead);
                if (read == 0)
                {
                    break;
                }
                bytesRead += read;
            }

            return bytesRead == PdfMagicBytes.Length && header.AsSpan().SequenceEqual(PdfMagicBytes);
        }
    }
}
