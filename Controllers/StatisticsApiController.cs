using MAS.Payments.ActionFilters;
using MAS.Payments.Infrastructure;
using MAS.Payments.Models;
using MAS.Payments.Queries;

using Microsoft.AspNetCore.Mvc;

namespace MAS.Payments.Controllers
{
    [Authorize]
    [Route("api/stats")]
    public class StatisticsApiController : BaseApiController
    {
        public StatisticsApiController(
            IResolver resolver
        ) : base(resolver)
        {
        }

        [HttpGet("[action]")]
        public GetStatisticsResponse GetStatistics([FromQuery]GetStatisticsRequest request)
        {
            GetStatisticsQuery query = null;
            long cachedUserId = GetCachedUserId();

            if (request.Year.HasValue)
            {
                query = new GetStatisticsQuery(cachedUserId, request.Year.Value, request.IncludeMeasurements);
            }
            else if (request.IsDatePeriodSpecified)
            {
                query = new GetStatisticsQuery(cachedUserId, request.From.Value, request.To.Value, request.IncludeMeasurements);
            }
            else
            {
                query = new GetStatisticsQuery(cachedUserId, request.IncludeMeasurements);
            }

            return QueryProcessor.Execute(query);
        }
    }
}