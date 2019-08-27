using System.Collections.Generic;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Models;
using MAS.Payments.Notifications;
using MAS.Payments.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MAS.Payments.Controllers
{
    [Route("api/stats")]
    public class StatisticsApiController : BaseApiController
    {
        public StatisticsApiController(
            ICommandProcessor commandProcessor,
            IQueryProcessor queryProcessor,
            INotificationProcessor notificationProcessor
        ) : base(commandProcessor, queryProcessor, notificationProcessor)
        {
        }

        [HttpGet("[action]")]
        public GetStatisticsResponse GetStatistics([FromQuery]GetStatisticsRequest request)
        {
            GetStatisticsQuery query = null;

            if (request.Year.HasValue)
            {
                query = new GetStatisticsQuery(request.Year.Value, request.IncludeMeasurements);
            }
            else if (request.IsDatePeriodSpecified)
            {
                query = new GetStatisticsQuery(request.From.Value, request.To.Value, request.IncludeMeasurements);
            }
            else
            {
                query = new GetStatisticsQuery(request.IncludeMeasurements);
            }

            return QueryProcessor.Execute(query);
        }
    }
}