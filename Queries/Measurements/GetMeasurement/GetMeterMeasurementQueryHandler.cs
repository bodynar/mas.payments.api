namespace MAS.Payments.Queries
{
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;

    internal class GetMeterMeasurementQueryHandler : BaseQueryHandler<GetMeterMeasurementQuery, GetMeterMeasurementResponse>
    {
        private IRepository<MeterMeasurement> Repository { get; }

        public GetMeterMeasurementQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<MeterMeasurement>();
        }

        public override async Task<GetMeterMeasurementResponse> HandleAsync(GetMeterMeasurementQuery query)
        {
            var item = await Repository.Get(query.Id);

            return new GetMeterMeasurementResponse
            {
                Id = item.Id,
                Measurement = item.Measurement,
                Diff = item.Diff,
                Comment = item.Comment,
                DateYear = item.Date.Year,
                DateMonth = item.Date.Month,
                IsSent = item.IsSent,
                MeterMeasurementTypeId = item.MeterMeasurementTypeId,
                MeasurementTypeName = item.MeasurementType.Name,
            };
        }
    }
}