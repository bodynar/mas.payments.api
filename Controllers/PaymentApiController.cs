namespace MAS.Payments.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MAS.Payments.Commands;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Models;
    using MAS.Payments.Queries;

    using Microsoft.AspNetCore.Mvc;

    using Newtonsoft.Json;

    [Route("api/payment")]
    public class PaymentApiController(
        IResolver resolver
    ) : BaseApiController(resolver)
    {
        #region Payment type

        [HttpGet("[action]")]
        public async Task<GetPaymentTypeResponse> GetPaymentType(long? id)
        {
            if (!id.HasValue || id.Value == default)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await QueryProcessor.Execute(new GetPaymentTypeQuery(id.Value));
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<GetPaymentTypesResponse>> GetPaymentTypes()
        {
            return await QueryProcessor.Execute(new GetPaymentTypesQuery());
        }

        [HttpPost("[action]")]
        public async Task AddPaymentType([FromBody] AddPaymentTypeRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await CommandProcessor.Execute(
                new AddPaymentTypeCommand(request.Name, request.Description, request.Company, request.Color));
        }

        [HttpPost("[action]")]
        public async Task UpdatePaymentType(UpdatePaymentTypeRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await CommandProcessor.Execute(
                new UpdatePaymentTypeCommand(request.Id, request.Name, request.Description, request.Company, request.Color)
            );
        }

        [HttpPost("[action]")]
        public async Task DeletePaymentType([FromBody] DeleteRecordRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await CommandProcessor.Execute(
                new DeletePaymentTypeCommand(request.Id));
        }

        #endregion

        #region Payment

        [HttpGet("[action]")]
        public async Task<IEnumerable<GetPaymentsResponse>> GetPayments([FromQuery] GetPaymentsRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return await QueryProcessor.Execute(
                new GetPaymentsQuery(request.Month, request.Year, request.PaymentTypeId,
                    request.Amount?.Exact, request.Amount?.Min, request.Amount?.Max));
        }

        [HttpPost("[action]")]
        public async Task AddPayment([FromBody] AddPaymentRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var paymentDate = new DateTime(request.Year, request.Month, 20, 0, 0, 0, DateTimeKind.Utc);

            await CommandProcessor.Execute(
                new AddPaymentCommand(
                    request.PaymentTypeId, request.Amount,
                    paymentDate, request.Description
                )
            );
        }

        [HttpPost("[action]")]
        public async Task AddGroup([FromBody] AddPaymentGroupRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var payments = JsonConvert.DeserializeObject<List<PaymentGroupRequestModel>>(request.Payments);

            if (payments?.Count == 0)
            {
                return;
            }

            var paymentDate = new DateTime(request.Year, request.Month, 20, 0, 0, 0, DateTimeKind.Utc);

            await CommandProcessor.Execute(
                new AddPaymentGroupCommand(
                    paymentDate,
                    payments.Select(x => new PaymentGroup(x.Amount, x.PaymentTypeId, x.Description))
                )
            );
        }

        [HttpPost("[action]")]
        public async Task UpdatePayment([FromBody] UpdatePaymentRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var paymentDate = new DateTime(request.Year, request.Month, 20, 0, 0, 0, DateTimeKind.Utc);

            await CommandProcessor.Execute(
                new UpdatePaymentCommand(
                    request.Id, request.PaymentTypeId, request.Amount,
                    paymentDate, request.Description
                )
            );
        }

        [HttpPost("[action]")]
        public async Task DeletePayment([FromBody] DeleteRecordRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await CommandProcessor.Execute(
                new DeletePaymentCommand(request.Id)
            );
        }

        [HttpPost("[action]")]
        public async Task Attach([FromForm] AttachFileApiModel request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await CommandProcessor.Execute(
                new AttachFileCommand(
                    request.File,
                    request.PaymentId,
                    request.FieldName
                )
            );
        }

        [HttpPost("[action]")]
        public async Task DeleteFile([FromBody]DeleteFileRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await CommandProcessor.Execute(new DeleteRelatedFileCommand(request.PaymentId, request.Mode));
        }

        #endregion
    }
}