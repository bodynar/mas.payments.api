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
    [Route("api/measures")]
    public class MeterMeasurementApiController : BaseApiController
    {
        public MeterMeasurementApiController(
            ICommandProcessor commandProcessor,
            IQueryProcessor queryProcessor,
            INotificationProcessor notificationProcessor
        ) : base(commandProcessor, queryProcessor, notificationProcessor)
        {
        }

        [HttpGet("[action]")]
        public IEnumerable<GetMeterMeasurementTypesResponse> GetMeasurementTypes()
        {
            return QueryProcessor.Execute(new GetMeterMeasurementTypesQuery());
        }

        [HttpGet("[action]")]
        public IEnumerable<GetMeterMeasurementsResponse> GetMeasurements([FromQuery]GetMeterMeasurementRequest filter)
        {
            return QueryProcessor.Execute(
                new GetMeterMeasurementsQuery(filter.Month, filter.MeasurementTypeId));
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

        [HttpPost("[action]")]
        public void UpdateMeasurementType(UpdateMeterMeasurementTypeRequest request)
        {
            CommandProcessor.Execute(
                new UpdateMeterMeasurementTypeCommand(request.Id, request.PaymentTypeId, request.Name, request.Description)
            );
        }

        [HttpPost("[action]")]
        public void UpdateMeasurement(UpdateMeterMeasurementRequest request)
        {
            CommandProcessor.Execute(
                new UpdateMeterMeasurementCommand(request.Id, request.MeterMeasurementTypeId, request.Date, request.Measurement, request.Comment)
            );
        }

        [HttpDelete("[action]")]
        public void DeleteMeasurementType(long measurementTypeId)
        {
            CommandProcessor.Execute(
                new DeleteMeterMeasurementTypeCommand(measurementTypeId));
        }

        [HttpDelete("[action]")]
        public void DeleteMeasurement(long measurementId)
        {
            CommandProcessor.Execute(
                new DeleteMeterMeasurementCommand(measurementId));
        }
    }
}