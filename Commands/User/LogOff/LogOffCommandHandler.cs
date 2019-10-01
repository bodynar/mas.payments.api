using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.Exceptions;
using MAS.Payments.Infrastructure.Specification;

namespace MAS.Payments.Commands
{
    internal class LogOffCommandHandler : BaseCommandHandler<LogOffCommand>
    {
        private IRepository<UserToken> Repository { get; }

        public LogOffCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<UserToken>();
        }

        public override void Handle(LogOffCommand command)
        {
            var userTokenEntity =
                Repository.Get(new CommonSpecification<UserToken>(x => x.Token == command.Token));

            if (userTokenEntity == null)
            {
                throw new EntityNotFoundException(typeof(UserToken));
            }

            Repository.Delete(userTokenEntity);
        }
    }
}