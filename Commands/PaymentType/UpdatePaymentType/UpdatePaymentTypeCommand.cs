using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class UpdatePaymentTypeCommand : UserCommand
    {
        public long Id { get; }

        public string Name { get; }

        public string Description { get; }

        public string Company { get; }

        public UpdatePaymentTypeCommand(long userId, long id, string name, string description, string company)
            : base(userId)
        {
            Id = id;
            Name = name;
            Description = description;
            Company = company;
        }
    }
}