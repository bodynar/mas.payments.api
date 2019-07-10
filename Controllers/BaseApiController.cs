using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.Query;
using Microsoft.AspNetCore.Mvc;

namespace MAS.Payments.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class BaseApiController : Controller
    {
        protected ICommandProcessor CommandProcessor { get; }

        protected IQueryProcessor QueryProcessor { get; }

        public BaseApiController(
            ICommandProcessor commandProcessor,
            IQueryProcessor queryProcessor
        )
        {
            CommandProcessor = commandProcessor;
            QueryProcessor = queryProcessor;
        }
    }
}
