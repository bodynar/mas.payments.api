namespace MAS.Payments.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MAS.Payments.Commands;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Models;
    using MAS.Payments.Queries;

    using Microsoft.AspNetCore.Mvc;

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
        public GetPaymentTypeResponse GetPaymentType(long? id)
        {
            if (!id.HasValue || id.Value == default)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return QueryProcessor.Execute(new GetPaymentTypeQuery(id.Value));
        }

        [HttpGet("[action]")]
        public IEnumerable<GetPaymentTypesResponse> GetPaymentTypes()
        {
            return QueryProcessor.Execute(new GetPaymentTypesQuery());
        }

        [HttpPost("[action]")]
        public void AddPaymentType([FromBody] AddPaymentTypeRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            CommandProcessor.Execute(
                new AddPaymentTypeCommand(request.Name, request.Description, request.Company, request.Color));
        }

        [HttpPost("[action]")]
        public void UpdatePaymentType(UpdatePaymentTypeRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            CommandProcessor.Execute(
                new UpdatePaymentTypeCommand(request.Id, request.Name, request.Description, request.Company, request.Color)
            );
        }

        [HttpPost("[action]")]
        public void DeletePaymentType([FromBody] DeleteRecordRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            CommandProcessor.Execute(
                new DeletePaymentTypeCommand(request.Id));
        }

        #endregion

        #region Payment

        [HttpGet("[action]")]
        public GetPaymentResponse GetPayment(long? id)
        {
            if (!id.HasValue || id.Value == default)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return QueryProcessor.Execute(new GetPaymentQuery(id.Value));
        }

        [HttpGet("[action]")]
        public IEnumerable<GetPaymentsResponse> GetPayments([FromQuery] GetPaymentsRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return QueryProcessor.Execute(
                new GetPaymentsQuery(request.Month, request.Year, request.PaymentTypeId,
                    request.Amount?.Exact, request.Amount?.Min, request.Amount?.Max));
        }

        [HttpPost("[action]")]
        public void AddPayment([FromBody] AddPaymentRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var paymentDate = new DateTime(request.Year, request.Month, 20);

            CommandProcessor.Execute(
                new AddPaymentCommand(request.PaymentTypeId, request.Amount, paymentDate, request.Description));
        }

        [HttpPost("[action]")]
        public void AddGroup([FromBody] AddPaymentGroupRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            CommandProcessor.Execute(new AddPaymentGroupCommand(request.Date, request.Payments.Select(x => new PaymentGroup(x.Amount, x.PaymentTypeId, x.Description))));
        }

        [HttpPost("[action]")]
        public void UpdatePayment([FromBody] UpdatePaymentRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var paymentDate = new DateTime(request.Year, request.Month, 20);

            CommandProcessor.Execute(
                new UpdatePaymentCommand(request.Id, request.PaymentTypeId, request.Amount, paymentDate, request.Description)
            );
        }

        [HttpPost("[action]")]
        public void DeletePayment([FromBody] DeleteRecordRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            CommandProcessor.Execute(
                new DeletePaymentCommand(request.Id)
            );
        }

        [HttpGet("[action]")]
        public GetPaymentAverageValueResponse GetAverageValues()
        {
            return QueryProcessor.Execute(new GetPaymentAverageValueQuery());
        }

        #endregion
    }
}