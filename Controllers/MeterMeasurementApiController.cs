namespace MAS.Payments.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MAS.Payments.Commands;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Models;
    using MAS.Payments.Queries;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/measurement")]
    public class MeterMeasurementApiController(
        IResolver resolver
    ) : BaseApiController(resolver)
    {

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
            ArgumentNullException.ThrowIfNull(request);

            CommandProcessor.Execute(
                new AddMeterMeasurementTypeCommand(request.PaymentTypeId, request.Name, request.Description, request.Color));
        }

        [HttpPost("[action]")]
        public void UpdateMeasurementType(UpdateMeterMeasurementTypeRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            CommandProcessor.Execute(
                new UpdateMeterMeasurementTypeCommand(request.Id, request.PaymentTypeId, request.Name, request.Description, request.Color)
            );
        }

        [HttpPost("[action]")]
        public void DeleteMeasurementType([FromBody] DeleteRecordRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

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
            ArgumentNullException.ThrowIfNull(filter);

            return QueryProcessor.Execute(
                new GetGroupedMeterMeasurementsQuery(filter.Month, filter.MeasurementTypeId, filter.Year));
        }

        [HttpGet("[action]")]
        public IEnumerable<GetMeterMeasurementsQueryResponse> GetMeasurements([FromQuery] GetMeterMeasurementRequest filter)
        {
            ArgumentNullException.ThrowIfNull(filter);

            return QueryProcessor.Execute(
                new GetMeterMeasurementsQuery(filter.Month, filter.MeasurementTypeId, filter.Year));
        }

        [HttpPost("[action]")]
        public void AddMeasurement([FromBody] AddMeasurementGroupRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            CommandProcessor.Execute(
                new AddMeasurementGroupCommand(
                    new DateTime(request.Year, request.Month, 1, 0, 0, 0, DateTimeKind.Utc),
                    request.Measurements.Select(x => 
                        new MeasurementGroup(x.TypeId, x.Value, x.Comment)
                    )
                )
            );
        }

        [HttpPost("[action]")]
        public void UpdateMeasurement(UpdateMeterMeasurementRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var meterMeasurementDate = new DateTime(request.Year, request.Month, 20, 0, 0, 0, DateTimeKind.Utc);

            CommandProcessor.Execute(
                new UpdateMeterMeasurementCommand(request.Id, request.TypeId, meterMeasurementDate, request.Value, request.Comment)
            );
        }

        [HttpPost("[action]")]
        public void DeleteMeasurement([FromBody] DeleteRecordRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            CommandProcessor.Execute(
                new DeleteMeterMeasurementCommand(request.Id));
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