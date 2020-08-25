using System;
using System.Collections.Generic;
using System.Linq;

using MAS.Payments.Commands;
using MAS.Payments.DataBase;
using MAS.Payments.Infrastructure;
using MAS.Payments.Models;
using MAS.Payments.Queries;
using MAS.Payments.Utilities;

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
                new AddMeterMeasurementTypeCommand(request.PaymentTypeId, request.Name, request.Description, request.Color));
        }

        [HttpPost("[action]")]
        public void UpdateMeasurementType(UpdateMeterMeasurementTypeRequest request)
        {
            CommandProcessor.Execute(
                new UpdateMeterMeasurementTypeCommand(request.Id, request.PaymentTypeId, request.Name, request.Description, request.Color)
            );
        }

        [HttpPost("[action]")]
        public void DeleteMeasurementType([FromBody]long measurementTypeId)
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
                new GetMeterMeasurementsQuery(filter.Month, filter.MeasurementTypeId, filter.Year));
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

        [HttpPost("[action]")]
        public void DeleteMeasurement([FromBody]long measurementId)
        {
            CommandProcessor.Execute(
                new DeleteMeterMeasurementCommand(measurementId));
        }

        [HttpPost("[action]")]
        public void SendMeasurements([FromBody]IEnumerable<long> measurementIdentifiers)
        {
            if (!measurementIdentifiers.Any())
            {
                throw new ArgumentException("No measurement identifiers not specified");
            }

            var recipientEmail = QueryProcessor.Execute(new GetNamedUserSettingQuery(DefaultUserSettings.EmailToSendMeasurements.ToString()));

            var isValidEmail = Validate.Email(recipientEmail.RawValue);

            if (!isValidEmail)
            {
                throw new ArgumentException($"User setting \"{recipientEmail.RawValue}\" isn't recognized as email.");
            }

            CommandProcessor.Execute(new SendMeasurementsCommand(recipientEmail.RawValue, measurementIdentifiers));
        }

        #endregion
    }
}