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

    [Route("api/payment")]
    public class PaymentApiController(
        IResolver resolver
    ) : BaseApiController(resolver)
    {
        #region Payment type

        [HttpGet("[action]")]
        public async Task<GetPaymentTypeResponse> GetPaymentTypeAsync(long? id)
        {
            if (!id.HasValue || id.Value == default)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await QueryProcessor.Execute(new GetPaymentTypeQuery(id.Value));
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<GetPaymentTypesResponse>> GetPaymentTypesAsync()
        {
            return await QueryProcessor.Execute(new GetPaymentTypesQuery());
        }

        [HttpPost("[action]")]
        public async Task AddPaymentTypeAsync([FromBody] AddPaymentTypeRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await CommandProcessor.Execute(
                new AddPaymentTypeCommand(request.Name, request.Description, request.Company, request.Color));
        }

        [HttpPost("[action]")]
        public async Task UpdatePaymentTypeAsync(UpdatePaymentTypeRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await CommandProcessor.Execute(
                new UpdatePaymentTypeCommand(request.Id, request.Name, request.Description, request.Company, request.Color)
            );
        }

        [HttpPost("[action]")]
        public async Task DeletePaymentTypeAsync([FromBody] DeleteRecordRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await CommandProcessor.Execute(
                new DeletePaymentTypeCommand(request.Id));
        }

        #endregion

        #region Payment

        [HttpGet("[action]")]
        public async Task<GetPaymentResponse> GetPaymentAsync(long? id)
        {
            if (!id.HasValue || id.Value == default)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await QueryProcessor.Execute(new GetPaymentQuery(id.Value));
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<GetPaymentsResponse>> GetPaymentsAsync([FromQuery] GetPaymentsRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return await QueryProcessor.Execute(
                new GetPaymentsQuery(request.Month, request.Year, request.PaymentTypeId,
                    request.Amount?.Exact, request.Amount?.Min, request.Amount?.Max));
        }

        [HttpPost("[action]")]
        public async Task AddPaymentAsync([FromBody] AddPaymentRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var paymentDate = new DateTime(request.Year, request.Month, 20, 0, 0, 0, DateTimeKind.Utc);

            await CommandProcessor.Execute(
                new AddPaymentCommand(request.PaymentTypeId, request.Amount, paymentDate, request.Description));
        }

        [HttpPost("[action]")]
        public async Task AddGroupAsync([FromBody] AddPaymentGroupRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await CommandProcessor.Execute(new AddPaymentGroupCommand(request.Date, request.Payments.Select(x => new PaymentGroup(x.Amount, x.PaymentTypeId, x.Description))));
        }

        [HttpPost("[action]")]
        public async Task UpdatePaymentAsync([FromBody] UpdatePaymentRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var paymentDate = new DateTime(request.Year, request.Month, 20, 0, 0, 0, DateTimeKind.Utc);

            await CommandProcessor.Execute(
                new UpdatePaymentCommand(request.Id, request.PaymentTypeId, request.Amount, paymentDate, request.Description)
            );
        }

        [HttpPost("[action]")]
        public async Task DeletePaymentAsync([FromBody] DeleteRecordRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await CommandProcessor.Execute(
                new DeletePaymentCommand(request.Id)
            );
        }

        #endregion
    }
}