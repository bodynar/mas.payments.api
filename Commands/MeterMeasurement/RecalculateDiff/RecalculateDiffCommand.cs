namespace MAS.Payments.Commands
{
    using MAS.Payments.Infrastructure.Command;

    public class RecalculateDiffCommand: ICommand
    {
        public bool ForAll { get; }

        public RecalculateDiffCommand(bool forAll = false)
        {
            ForAll = forAll;
        }
    }
}
