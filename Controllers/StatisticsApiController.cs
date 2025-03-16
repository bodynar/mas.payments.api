namespace MAS.Payments.Controllers
{
    using System;

    using MAS.Payments.Infrastructure;
    using MAS.Payments.Models;
    using MAS.Payments.Queries;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/stats")]
    public class StatisticsApiController(
        IResolver resolver
    ) : BaseApiController(resolver)
    {
        [HttpGet("[action]")]
        public GetPaymentStatisticsResponse GetPaymentsStatistics([FromQuery] GetPaymentsStatisticsRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("Year and Payment type must be specified.");
            }

            return QueryProcessor.Execute(new GetPaymentStatisticsQuery(request.From, request.To, request.PaymentTypeId));
        }

        [HttpGet("[action]")]
        public GetMeasurementStatisticsQueryResponse GetMeasurementStatistics([FromQuery] GetMeasurementStatisticsRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("Year and Measurement type must be specified.");
            }

            return QueryProcessor.Execute(new GetMeasurementStatisticsQuery(request.From, request.To, request.MeasurementTypeId));
        }
    }
}