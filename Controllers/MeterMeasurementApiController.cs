namespace MAS.Payments.Controllers
{
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
        public GetMeterMeasurementTypeResponse GetMeasurementType(long? id)
        {
            if (!id.HasValue || id.Value == default)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return QueryProcessor.Execute(new GetMeterMeasurementTypeQuery(id.Value));
        }

        [HttpGet("[action]")]
        public IEnumerable<GetMeterMeasurementTypesResponse> GetMeasurementTypes()
        {
            return QueryProcessor.Execute(new GetMeterMeasurementTypesQuery());
        }

        [HttpPost("[action]")]
        public void AddMeasurementType(AddMeterMeasurementTypeRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            CommandProcessor.Execute(
                new AddMeterMeasurementTypeCommand(request.PaymentTypeId, request.Name, request.Description, request.Color));
        }

        [HttpPost("[action]")]
        public void UpdateMeasurementType(UpdateMeterMeasurementTypeRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            CommandProcessor.Execute(
                new UpdateMeterMeasurementTypeCommand(request.Id, request.PaymentTypeId, request.Name, request.Description, request.Color)
            );
        }

        [HttpPost("[action]")]
        public void DeleteMeasurementType([FromBody] DeleteRecordRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            CommandProcessor.Execute(
                new DeleteMeterMeasurementTypeCommand(request.Id));
        }

        #endregion

        #region Measurement

        [HttpGet("[action]")]
        public GetMeterMeasurementResponse GetMeasurement(long? id)
        {
            if (!id.HasValue || id.Value == default)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return QueryProcessor.Execute(new GetMeterMeasurementQuery(id.Value));
        }

        [HttpGet("[action]")]
        public IEnumerable<GetGroupedMeterMeasurementsResponse> GetGroupedMeasurements([FromQuery] GetMeterMeasurementRequest filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            return QueryProcessor.Execute(
                new GetGroupedMeterMeasurementsQuery(filter.Month, filter.MeasurementTypeId, filter.Year));
        }

        [HttpGet("[action]")]
        public IEnumerable<GetMeterMeasurementsQueryResponse> GetMeasurements([FromQuery] GetMeterMeasurementRequest filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            return QueryProcessor.Execute(
                new GetMeterMeasurementsQuery(filter.Month, filter.MeasurementTypeId, filter.Year));
        }

        [HttpPost("[action]")]
        public void AddMeasurement([FromBody] AddMeasurementGroupRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            CommandProcessor.Execute(
                new AddMeasurementGroupCommand(
                    new DateTime(request.Year, request.Month, 1),
                    request.Measurements.Select(x => 
                        new MeasurementGroup(x.TypeId, x.Value, x.Comment)
                    )
                )
            );
        }

        [HttpPost("[action]")]
        public void UpdateMeasurement(UpdateMeterMeasurementRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var meterMeasurementDate = new DateTime(request.Year, request.Month, 20);

            CommandProcessor.Execute(
                new UpdateMeterMeasurementCommand(request.Id, request.TypeId, meterMeasurementDate, request.Value, request.Comment)
            );
        }

        [HttpPost("[action]")]
        public void DeleteMeasurement([FromBody] DeleteRecordRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            CommandProcessor.Execute(
                new DeleteMeterMeasurementCommand(request.Id));
        }

        [HttpPost("[action]")]
        public void SendMeasurements([FromBody] IEnumerable<long> measurementIdentifiers)
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

        [HttpGet("withoutDiff")]
        public int GetMeasurementsWithoutDiff()
        {
            return QueryProcessor.Execute(new GetMeasurementsWithoutDiffQuery());
        }

        [HttpPost("[action]")]
        public IEnumerable<string> UpdateDiff()
        {
            var command = new RecalculateDiffCommand(false);

            CommandProcessor.Execute(command);

            return command.Warnings;
        }

        #endregion
    }
}