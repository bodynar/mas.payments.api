namespace MAS.Payments.Queries.Measurements.GetMeasurementsWithoutDiff
{
    using System.Linq;

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

        public override int Handle(GetMeasurementsWithoutDiffQuery query)
        {
            return Repository.Where(new MeterMeasurementSpec.WithoutDiff()).Count();
        }
    }
}
