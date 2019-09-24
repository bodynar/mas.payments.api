using System.Collections.Generic;
using System.Linq;
using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Queries
{
    internal class GetUsersQueryHandler : BaseQueryHandler<GetUsersQuery, IEnumerable<GetUsersQueryResponse>>
    {
        private IRepository<User> Repository { get; }

        public GetUsersQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<User>();
        }

        public override IEnumerable<GetUsersQueryResponse> Handle(GetUsersQuery query)
        {
            return
                Repository
                    .GetAll()
                    .ToList()
                    .Select(x => new GetUsersQueryResponse
                    {
                        Id = x.Id,
                        Login = x.Login,
                        Email = x.Email,
                        Settings = new GetUsersQueryResponse.UserSettings
                        {
                            MeasurementsEmailToSend = x.UserSettings.MeasurementsEmailToSend,
                            DisplayMeasurementNotification = x.UserSettings.DisplayMeasurementNotification
                        }
                    });
        }
    }
}