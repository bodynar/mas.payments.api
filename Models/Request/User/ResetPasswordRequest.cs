namespace MAS.Payments.Models
{
    public class ResetPasswordRequest
    {
        public string Login { get; set; }

        public string Email { get; set; }
    }
}