namespace MAS.Payments.Queries
{
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;

    internal class GetPaymentFileQueryHandler : BaseQueryHandler<GetPaymentFileQuery, GetPaymentFileResponse>
    {
        private IRepository<PaymentFile> Repository { get; }

        public GetPaymentFileQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<PaymentFile>();
        }

        public override async Task<GetPaymentFileResponse> HandleAsync(GetPaymentFileQuery query)
        {
            var file = await Repository.Get(query.Id);

            if (file == null)
            {
                return null;
            }

            return new GetPaymentFileResponse
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                Data = file.Data,
            };
        }
    }
}
