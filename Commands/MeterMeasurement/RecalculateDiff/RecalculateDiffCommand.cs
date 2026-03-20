namespace MAS.Payments.Commands
{
    using MAS.Payments.Infrastructure.Command;

    public class RecalculateDiffCommand(
        bool forAll = false
    ) : ICommand
    {
        public bool ForAll { get; } = forAll;
    }
}
