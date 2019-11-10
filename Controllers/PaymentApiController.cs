using System.Collections.Generic;
using MAS.Payments.ActionFilters;
using MAS.Payments.Commands;
using MAS.Payments.Infrastructure;
using MAS.Payments.Models;
using MAS.Payments.Queries;

using Microsoft.AspNetCore.Mvc;

namespace MAS.Payments.Controllers
{
    [Authorize]
    [Route("api/payment")]
    public class PaymentApiController : BaseApiController
    {
        public PaymentApiController(
            IResolver resolver
        ) : base(resolver)
        {
        }

        #region Payment type

        [HttpGet("[action]")]
        public GetPaymentTypeResponse GetPaymentType(long id)
        {
            return QueryProcessor.Execute(new GetPaymentTypeQuery(id));
        }

        [HttpGet("[action]")]
        public IEnumerable<GetPaymentTypesResponse> GetPaymentTypes()
        {
            return QueryProcessor.Execute(new GetPaymentTypesQuery());
        }

        [HttpPost("[action]")]
        public void AddPaymentType([FromBody]AddPaymentTypeRequest request)
        {
            CommandProcessor.Execute(
                new AddPaymentTypeCommand(request.Name, request.Description, request.Company));
        }

        [HttpPost("[action]")]
        public void UpdatePaymentType(UpdatePaymentTypeRequest request)
        {
            CommandProcessor.Execute(
                new UpdatePaymentTypeCommand(request.Id, request.Name, request.Description, request.Company)
            );
        }

        [HttpDelete("[action]")]
        public void DeletePaymentType(long paymentTypeId)
        {
            CommandProcessor.Execute(
                new DeletePaymentTypeCommand(paymentTypeId));
        }

        #endregion

        #region Payment

        [HttpGet("[action]")]
        public GetPaymentResponse GetPayment(long id)
        {
            return QueryProcessor.Execute(new GetPaymentQuery(id));
        }

        [HttpGet("[action]")]
        public IEnumerable<GetPaymentsResponse> GetPayments([FromQuery]GetPaymentsRequest request)
        {
            return QueryProcessor.Execute(
                new GetPaymentsQuery(request.Month, request.PaymentTypeId,
                    request.Amount?.Exact, request.Amount?.Min, request.Amount?.Max));
        }

        [HttpPost("[action]")]
        public void AddPayment([FromBody]AddPaymentRequest request)
        {
            CommandProcessor.Execute(
                new AddPaymentCommand(request.PaymentTypeId, request.Amount, request.Date, request.Description));
        }

        [HttpPost("[action]")]
        public void UpdatePayment(UpdatePaymentRequest request)
        {
            CommandProcessor.Execute(
                new UpdatePaymentCommand(request.Id, request.PaymentTypeId, request.Amount, request.Date, request.Description)
            );
        }

        [HttpDelete("[action]")]
        public void DeletePayment(long paymentId)
        {
            CommandProcessor.Execute(
                new DeletePaymentCommand(paymentId));
        }

        #endregion

    }
}