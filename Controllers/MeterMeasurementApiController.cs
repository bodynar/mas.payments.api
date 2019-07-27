using System.Collections.Generic;
using MAS.Payments.Commands;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Models;
using MAS.Payments.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MAS.Payments.Controllers
{
    [Route("api/measures")]
    public class MeterMeasurementApiController : BaseApiController
    {
        public MeterMeasurementApiController(
            ICommandProcessor commandProcessor,
            IQueryProcessor queryProcessor
        ) : base(commandProcessor, queryProcessor)
        {
        }

        [HttpGet("[action]")]
        public IEnumerable<GetMeterMeasurementTypesResponse> GetMeasurementTypes()
        {
            return QueryProcessor.Execute(new GetMeterMeasurementTypesQuery());
        }

        [HttpGet("[action]")]
        public IEnumerable<GetMeterMeasurementsResponse> GetMeasurements()
        {
            return QueryProcessor.Execute(new GetMeterMeasurementsQuery());
        }

        [HttpPost("[action]")]
        public void AddMeasurement(AddMeterMeasurementRequest request)
        {
            CommandProcessor.Execute(
                new AddMeterMeasurementCommand(request.MeterMeasurementTypeId, request.Date, request.Measurement, request.Comment));
        }

        [HttpPost("[action]")]
        public void AddMeasurementType(AddMeterMeasurementTypeRequest request)
        {
            CommandProcessor.Execute(
                new AddMeterMeasurementTypeCommand(request.PaymentTypeId, request.Name, request.Description));
        }
    }
}