namespace MAS.Payments.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MAS.Payments.Commands;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Models;
    using MAS.Payments.Queries;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/paymentGroupTemplate")]
    public class PaymentGroupTemplateApiController(
        IResolver resolver
    ) : BaseApiController(resolver)
    {
        [HttpGet("[action]")]
        public async Task<GetPaymentGroupTemplateResponse> GetAsync(Guid? id)
        {
            if (!id.HasValue || id.Value == default)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await QueryProcessor.Execute(new GetPaymentGroupTemplateQuery(id.Value));
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<GetPaymentGroupTemplatesResponse>> GetAllAsync()
        {
            return await QueryProcessor.Execute(new GetPaymentGroupTemplatesQuery());
        }

        [HttpPost("[action]")]
        public async Task AddAsync([FromBody] AddPaymentGroupTemplateRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await CommandProcessor.Execute(
                new AddPaymentGroupTemplateCommand(request.Name, request.Description, request.PaymentTypeIds));
        }

        [HttpPost("[action]")]
        public async Task UpdateAsync([FromBody] UpdatePaymentGroupTemplateRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await CommandProcessor.Execute(
                new UpdatePaymentGroupTemplateCommand(request.Id, request.Name, request.Description, request.PaymentTypeIds));
        }

        [HttpPost("[action]")]
        public async Task DeleteAsync([FromBody] DeleteRecordRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await CommandProcessor.Execute(
                new DeletePaymentGroupTemplateCommand(request.Id));
        }
    }
}
