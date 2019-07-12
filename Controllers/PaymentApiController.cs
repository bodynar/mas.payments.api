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
    public class paymentController : BaseApiController
    {
        public paymentController(
            ICommandProcessor commandProcessor,
            IQueryProcessor queryProcessor
        ) : base(commandProcessor, queryProcessor)
        {
        }

        [HttpPost]
        public void AddPaymentType([FromBody]AddPaymentTypeRequest request)
        {
            CommandProcessor.Execute(
                new AddPaymentTypeCommand(request.Name, request.Description, request.Company));
        }

        [HttpGet]
        public IEnumerable<GetPaymentTypesResponse> GetPaymentTypes()
        {
            return QueryProcessor.Execute(new GetPaymentTypesQuery());
        }
    }
}