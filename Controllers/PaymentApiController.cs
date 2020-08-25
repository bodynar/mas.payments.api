using System;
using System.Collections.Generic;

using MAS.Payments.Commands;
using MAS.Payments.Infrastructure;
using MAS.Payments.Models;
using MAS.Payments.Queries;

using Microsoft.AspNetCore.Mvc;

namespace MAS.Payments.Controllers
{
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
                new AddPaymentTypeCommand(request.Name, request.Description, request.Company, request.Color));
        }

        [HttpPost("[action]")]
        public void UpdatePaymentType(UpdatePaymentTypeRequest request)
        {
            CommandProcessor.Execute(
                new UpdatePaymentTypeCommand(request.Id, request.Name, request.Description, request.Company, request.Color)
            );
        }

        [HttpPost("[action]")]
        public void DeletePaymentType([FromBody]long paymentTypeId)
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
                new GetPaymentsQuery(request.Month, request.Year, request.PaymentTypeId,
                    request.Amount?.Exact, request.Amount?.Min, request.Amount?.Max));
        }

        [HttpPost("[action]")]
        public void AddPayment([FromBody]AddPaymentRequest request)
        {
            var paymentDate = new DateTime(request.Year, request.Month, 20);

            CommandProcessor.Execute(
                new AddPaymentCommand(request.PaymentTypeId, request.Amount, paymentDate, request.Description));
        }

        [HttpPost("[action]")]
        public void UpdatePayment(UpdatePaymentRequest request)
        {
            var paymentDate = new DateTime(request.Year, request.Month, 20);

            CommandProcessor.Execute(
                new UpdatePaymentCommand(request.Id, request.PaymentTypeId, request.Amount, paymentDate, request.Description)
            );
        }

        [HttpPost("[action]")]
        public void DeletePayment([FromBody]long paymentId)
        {
            CommandProcessor.Execute(
                new DeletePaymentCommand(paymentId));
        }

        #endregion
    }
}