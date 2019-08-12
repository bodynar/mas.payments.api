using System.Collections.Generic;
using MAS.Payments.Commands;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Models;
using MAS.Payments.Notifications;
using MAS.Payments.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MAS.Payments.Controllers
{
    [Route("api/payment")]
    public class PaymentApiController : BaseApiController
    {
        public PaymentApiController(
            ICommandProcessor commandProcessor,
            IQueryProcessor queryProcessor,
            INotificationProcessor notificationProcessor
        ) : base(commandProcessor, queryProcessor, notificationProcessor)
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

        [HttpPost("[action]")]
        public void UpdatePaymentType(UpdatePaymentTypeRequest request)
        {
            CommandProcessor.Execute(
                new UpdatePaymentTypeCommand(request.Id, request.Name, request.Description, request.Company)
            );
        }

        [HttpPost("[action]")]
        public void UpdatePayment(UpdatePaymentRequest request)
        {
            CommandProcessor.Execute(
                new UpdatePaymentCommand(request.Id, request.PaymentTypeId, request.Amount, request.Date, request.Description)
            );
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

        [HttpDelete("[action]")]
        public void DeletePaymentType(long paymentTypeId)
        {
            CommandProcessor.Execute(
                new DeletePaymentTypeCommand(paymentTypeId));
        }

        [HttpDelete("[action]")]
        public void DeletePayment(long paymentId)
        {
            CommandProcessor.Execute(
                new DeletePaymentCommand(paymentId));
        }
    }
}