namespace MAS.Payments.Models
{
    public class CheckResult
    {
        protected bool isSuccess;

        public virtual bool IsSuccess
            => isSuccess;

        public string ErrorMessage { get; }

        protected CheckResult() { }

        protected CheckResult(bool isSuccess, string error = null)
        {
            this.isSuccess = isSuccess;
            ErrorMessage = error;
        }

        public static CheckResult Success()
        {
            return new CheckResult(true);
        }

        public static CheckResult Failure(string errorMessage)
        {
            return new CheckResult(false, errorMessage);
        }
    }
}