namespace MAS.Payments.Commands
{
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

        public override void Handle(AddUserSettingCommand command)
        {
            Repository.Add(new UserSettings
            {
                Name = command.Name,
                RawValue = command.RawValue,
                TypeName = command.TypeName,
            });
        }
    }
}