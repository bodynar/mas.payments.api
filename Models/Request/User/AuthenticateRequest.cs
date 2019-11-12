namespace MAS.Payments.Models
{
    public class AuthenticateRequest
    {
        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public bool RememberMe { get; set; }
    }
}