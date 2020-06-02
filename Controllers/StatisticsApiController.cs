namespace MAS.Payments.Controllers
{
    using System;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Models;
    using MAS.Payments.Queries;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/stats")]
    public class StatisticsApiController : BaseApiController
    {
        public StatisticsApiController(
            IResolver resolver
        ) : base(resolver)
        {
        }

        [HttpGet("[action]")]
        public GetStatisticsResponse GetPaymentsStatistics([FromQuery]GetStatisticsRequest request)
        {
            if (request == null || !request.Year.HasValue || !request.PaymentTypeId.HasValue)
            {
                throw new Exception("Year and Payment type must be specified.");
            }

            return QueryProcessor.Execute(new GetStatisticsQuery(request.Year.Value, request.PaymentTypeId.Value));
        }
    }
}