namespace MAS.Payments.Commands
{
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;

    internal class AddUserSettingCommandHandler : BaseCommandHandler<AddUserSettingCommand>
    {
        private IRepository<UserSettings> Repository { get; }

        public AddUserSettingCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<UserSettings>();
        }

        public override async Task HandleAsync(AddUserSettingCommand command)
        {
            await Repository.Add(new UserSettings
            {
                Name = command.Name,
                RawValue = command.RawValue,
                TypeName = command.TypeName,
            });
        }
    }
}