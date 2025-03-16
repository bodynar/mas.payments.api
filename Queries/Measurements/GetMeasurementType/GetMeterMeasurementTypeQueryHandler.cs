namespace MAS.Payments.Queries
{
    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;

    internal class GetMeterMeasurementTypeQueryHandler : BaseQueryHandler<GetMeterMeasurementTypeQuery, GetMeterMeasurementTypeResponse>
    {
        private IRepository<MeterMeasurementType> Repository { get; }

        public GetMeterMeasurementTypeQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<MeterMeasurementType>();
        }

        public override GetMeterMeasurementTypeResponse Handle(GetMeterMeasurementTypeQuery query)
        {
            var item = Repository.Get(query.Id);

            return new GetMeterMeasurementTypeResponse
            {
                Id = item.Id,
                SystemName = item.SystemName,
                Name = item.Name,
                Description = item.Description,
                PaymentTypeId = item.PaymentTypeId,
                PaymentTypeName = item.PaymentType.Name,
                Color = item.Color,
            };
        }
    }
}