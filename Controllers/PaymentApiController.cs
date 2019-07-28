using System.Collections.Generic;
using MAS.Payments.Commands;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Models;
using MAS.Payments.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MAS.Payments.Controllers
{
    [Route("api/payment")]
    public class PaymentApiController : BaseApiController
    {
        public PaymentApiController(
            ICommandProcessor commandProcessor,
            IQueryProcessor queryProcessor
        ) : base(commandProcessor, queryProcessor)
        {
        }

        [HttpPost("[action]")]
        public void AddPaymentType([FromBody]AddPaymentTypeRequest request)
        {
            CommandProcessor.Execute(
                new AddPaymentTypeCommand(request.Name, request.Description, request.Company));
        }

        [HttpPost("[action]")]
        public void AddPayment([FromBody]AddPaymentRequest request)
        {
            CommandProcessor.Execute(
                new AddPaymentCommand(request.PaymentTypeId, request.Amount, request.Date, request.Description));
        }

        [HttpGet("[action]")]
        public IEnumerable<GetPaymentTypesResponse> GetPaymentTypes()
        {
            return QueryProcessor.Execute(new GetPaymentTypesQuery());
        }

        [HttpGet("[action]")]
        public IEnumerable<GetPaymentsResponse> GetPayments([FromQuery]GetPaymentsRequest request)
        {
            return QueryProcessor.Execute(
                new GetPaymentsQuery(request.Month, request.PaymentTypeId,
                    request.Amount?.Exact, request.Amount?.Min, request.Amount?.Max));
        }
    }
}