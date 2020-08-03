namespace MAS.Payments.Utilities
{
    using System.ComponentModel.DataAnnotations;

    public static class Validate
    {
        public static bool Email(string email) 
            => new EmailAddressAttribute().IsValid(email);
    }
}