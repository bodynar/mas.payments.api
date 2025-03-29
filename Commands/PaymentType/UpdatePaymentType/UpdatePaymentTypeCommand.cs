namespace MAS.Payments.Commands
{
    using MAS.Payments.Infrastructure.Command;

    public class UpdatePaymentTypeCommand(
        long id,
        string name,
        string description,
        string company,
        string color
    ) : ICommand
    {
        public long Id { get; } = id;

        public string Name { get; } = name;

        public string Description { get; } = description;

        public string Company { get; } = company;

        public string Color { get; set; } = color;
    }
}