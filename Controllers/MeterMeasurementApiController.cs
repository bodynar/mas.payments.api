namespace MAS.Payments.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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
        public async Task<GetMeterMeasurementTypeResponse> GetMeasurementType(long? id)
        {
            if (!id.HasValue || id.Value == default)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await QueryProcessor.Execute(new GetMeterMeasurementTypeQuery(id.Value));
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<GetMeterMeasurementTypesResponse>> GetMeasurementTypes()
        {
            return await QueryProcessor.Execute(new GetMeterMeasurementTypesQuery());
        }

        [HttpPost("[action]")]
        public async Task AddMeasurementType(AddMeterMeasurementTypeRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await CommandProcessor.Execute(
                new AddMeterMeasurementTypeCommand(request.PaymentTypeId, request.Name, request.Description, request.Color));
        }

        [HttpPost("[action]")]
        public async Task UpdateMeasurementType(UpdateMeterMeasurementTypeRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await CommandProcessor.Execute(
                new UpdateMeterMeasurementTypeCommand(request.Id, request.PaymentTypeId, request.Name, request.Description, request.Color)
            );
        }

        [HttpPost("[action]")]
        public async Task DeleteMeasurementType([FromBody] DeleteRecordRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await CommandProcessor.Execute(
                new DeleteMeterMeasurementTypeCommand(request.Id));
        }

        #endregion

        #region Measurement

        [HttpGet("[action]")]
        public async Task<GetMeterMeasurementResponse> GetMeasurement(long? id)
        {
            if (!id.HasValue || id.Value == default)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await QueryProcessor.Execute(new GetMeterMeasurementQuery(id.Value));
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<GetGroupedMeterMeasurementsResponse>> GetGroupedMeasurements([FromQuery] GetMeterMeasurementRequest filter)
        {
            ArgumentNullException.ThrowIfNull(filter);

            return await QueryProcessor.Execute(
                new GetGroupedMeterMeasurementsQuery(filter.Month, filter.MeasurementTypeId, filter.Year));
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<GetMeterMeasurementsQueryResponse>> GetMeasurements([FromQuery] GetMeterMeasurementRequest filter)
        {
            ArgumentNullException.ThrowIfNull(filter);

            return await QueryProcessor.Execute(
                new GetMeterMeasurementsQuery(filter.Month, filter.MeasurementTypeId, filter.Year));
        }

        [HttpPost("[action]")]
        public async Task AddMeasurement([FromBody] AddMeasurementGroupRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await CommandProcessor.Execute(
                new AddMeasurementGroupCommand(
                    new DateTime(request.Year, request.Month, 1, 0, 0, 0, DateTimeKind.Utc),
                    request.Measurements.Select(x => 
                        new MeasurementGroup(x.TypeId, x.Value, x.Comment)
                    )
                )
            );
        }

        [HttpPost("[action]")]
        public async Task UpdateMeasurement(UpdateMeterMeasurementRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var meterMeasurementDate = new DateTime(request.Year, request.Month, 20, 0, 0, 0, DateTimeKind.Utc);

            await CommandProcessor.Execute(
                new UpdateMeterMeasurementCommand(request.Id, request.TypeId, meterMeasurementDate, request.Value, request.Comment)
            );
        }

        [HttpPost("[action]")]
        public async Task DeleteMeasurement([FromBody] DeleteRecordRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await CommandProcessor.Execute(
                new DeleteMeterMeasurementCommand(request.Id));
        }

        [HttpGet("withoutDiff")]
        public async Task<int> GetMeasurementsWithoutDiff()
        {
            return await QueryProcessor.Execute(new GetMeasurementsWithoutDiffQuery());
        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<string>> UpdateDiff()
        {
            var command = new RecalculateDiffCommand(false);

            await CommandProcessor.Execute(command);

            return command.Warnings;
        }

        #endregion
    }
}