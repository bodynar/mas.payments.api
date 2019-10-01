using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.Exceptions;
using MAS.Payments.Infrastructure.Specification;

namespace MAS.Payments.Commands
{
    internal class ConfirmRegistrationCommandHandler : BaseCommandHandler<ConfirmRegistrationCommand>
    {
        private IRepository<UserToken> Repository { get; }

        public ConfirmRegistrationCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<UserToken>();
        }

        public override void Handle(ConfirmRegistrationCommand command)
        {
            var userToken = Repository.Get(new CommonSpecification<UserToken>(x => x.Token == command.Token));

            if (userToken != null)
            {
                userToken.User.IsActive = true;

                Repository.Delete(userToken.Id);
            }
            else
            {
                throw new CommandExecutionException("Token not found");
            }
        }
    }
}