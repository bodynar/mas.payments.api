namespace MAS.Payments.Models
{
    public class RegistrationRequest
    {
        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}