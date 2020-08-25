using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class UpdatePaymentTypeCommand : ICommand
    {
        public long Id { get; }

        public string Name { get; }

        public string Description { get; }

        public string Company { get; }

        public string Color { get; set; }

        public UpdatePaymentTypeCommand(long id, string name, string description, string company, string color)
        {
            Id = id;
            Name = name;
            Description = description;
            Company = company;
            Color = color;
        }
    }
}