namespace MAS.Payments.Queries.Measurements.GetMeasurementsWithoutDiff
{
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;

    internal class GetMeasurementsWithoutDiffQueryHandler : BaseQueryHandler<GetMeasurementsWithoutDiffQuery, int>
    {
        private IRepository<MeterMeasurement> Repository { get; }

        public GetMeasurementsWithoutDiffQueryHandler(IResolver resolver)
            : base(resolver)
        {
            Repository = GetRepository<MeterMeasurement>();
        }

        public override async Task<int> HandleAsync(GetMeasurementsWithoutDiffQuery query)
        {
            return await Repository.Count(new MeterMeasurementSpec.WithoutDiff());
        }
    }
}
