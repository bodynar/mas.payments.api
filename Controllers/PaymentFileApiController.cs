namespace MAS.Payments.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
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

        [HttpPost("[action]")]
        [Consumes("multipart/form-data")]
        public async Task UploadFileAsync(
            IFormFile file,
            [FromForm] long? paymentId,
            [FromForm] long? paymentGroupId)
        {
            ArgumentNullException.ThrowIfNull(file);

            if (!string.Equals(file.ContentType, AllowedContentType, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Only PDF files are allowed.");
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var data = memoryStream.ToArray();

            await CommandProcessor.Execute(
                new UploadPaymentFileCommand(file.FileName, file.ContentType, data, paymentId, paymentGroupId));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetFileAsync(long? id)
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
        public async Task<IActionResult> ViewFileAsync(long? id)
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

            await CommandProcessor.Execute(new DeletePaymentFilesCommand(request.Ids));
        }
    }
}
