namespace MAS.Payments.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;

    using Microsoft.EntityFrameworkCore;

    internal class GetPaymentFilesQueryHandler : BaseQueryHandler<GetPaymentFilesQuery, IEnumerable<GetPaymentFilesResponse>>
    {
        private IRepository<PaymentFile> Repository { get; }

        public GetPaymentFilesQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<PaymentFile>();
        }

        public override async Task<IEnumerable<GetPaymentFilesResponse>> HandleAsync(GetPaymentFilesQuery query)
        {
            return await Repository
                .GetAll()
                .Select(f => new GetPaymentFilesResponse
                {
                    Id = f.Id,
                    FileName = f.FileName,
                    FileSize = f.FileSize,
                    UploadedAt = f.CreatedOn,
                    PaymentId = f.PaymentId,
                    PaymentGroupId = f.PaymentGroupId,
                    LinkedEntity = f.PaymentId != null ? "Payment" : "PaymentGroup",
                })
                .OrderByDescending(f => f.UploadedAt)
                .ToListAsync();
        }
    }
}
