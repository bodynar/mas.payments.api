namespace MAS.Payments.Queries
{
    using System.Collections.Generic;
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Projector;
    using MAS.Payments.Infrastructure.Query;
    using MAS.Payments.Projectors;

    internal class GetPaymentTypesQueryHandler : BaseQueryHandler<GetPaymentTypesQuery, IEnumerable<GetPaymentTypesResponse>>
    {
        private IRepository<PaymentType> Repository { get; }

        private IProjector<PaymentType, GetPaymentTypesResponse> Projector { get; }

        public GetPaymentTypesQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<PaymentType>();
            Projector = new Projector.ToFlat<PaymentType, GetPaymentTypesResponse>();
        }

        public override IEnumerable<GetPaymentTypesResponse> Handle(GetPaymentTypesQuery query)
        {
            return Repository
                   .GetAll()
                   .ToList()
                   .Select(item => {
                       var mapped = Projector.Project(item);
                       mapped.HasRelatedPayments = item.Payments.Any();
                       mapped.HasRelatedMeasurementTypes = item.MeasurementTypes.Any();

                       return mapped;
                   });
        }
    }
}