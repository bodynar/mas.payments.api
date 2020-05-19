using System;
using System.Collections.Generic;

using MAS.Payments.Commands;
using MAS.Payments.Infrastructure;
using MAS.Payments.Models;
using MAS.Payments.Queries;

using Microsoft.AspNetCore.Mvc;

namespace MAS.Payments.Controllers
{
    [Route("api/measurement")]
    public class MeterMeasurementApiController : BaseApiController
    {
        public MeterMeasurementApiController(
            IResolver resolver
        ) : base(resolver)
        {
        }

        #region Measurement Type

        [HttpGet("[action]")]
        public GetMeterMeasurementTypeResponse GetMeasurementType(long id)
        {
            return QueryProcessor.Execute(new GetMeterMeasurementTypeQuery(id));
        }

        [HttpGet("[action]")]
        public IEnumerable<GetMeterMeasurementTypesResponse> GetMeasurementTypes()
        {
            return QueryProcessor.Execute(new GetMeterMeasurementTypesQuery());
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

        [HttpDelete("[action]")]
        public void DeleteMeasurementType(long measurementTypeId)
        {
            CommandProcessor.Execute(
                new DeleteMeterMeasurementTypeCommand(measurementTypeId));
        }

        #endregion

        #region Measurement

        [HttpGet("[action]")]
        public GetMeterMeasurementResponse GetMeasurement(long id)
        {
            return QueryProcessor.Execute(new GetMeterMeasurementQuery(id));
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
            var meterMeasurementDate = new DateTime(request.Year, request.Month, 20);

            CommandProcessor.Execute(
                new AddMeterMeasurementCommand(request.MeterMeasurementTypeId, meterMeasurementDate, request.Measurement, request.Comment));
        }

        [HttpPost("[action]")]
        public void UpdateMeasurement(UpdateMeterMeasurementRequest request)
        {
            var meterMeasurementDate = new DateTime(request.Year, request.Month, 20);

            CommandProcessor.Execute(
                new UpdateMeterMeasurementCommand(request.Id, request.MeterMeasurementTypeId, meterMeasurementDate, request.Measurement, request.Comment)
            );
        }

        [HttpDelete("[action]")]
        public void DeleteMeasurement(long measurementId)
        {
            CommandProcessor.Execute(
                new DeleteMeterMeasurementCommand(measurementId));
        }

        [HttpPost("[action]")]
        public void SendMeasurements([FromBody]IEnumerable<long> measurementIdentifiers)
        {
            CommandProcessor.Execute(new SendMeasurementsCommand(null, measurementIdentifiers));
        }

        #endregion
    }
}