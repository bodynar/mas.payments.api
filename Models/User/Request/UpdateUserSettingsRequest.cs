namespace MAS.Payments.Models
{
    public class UpdateUserSettingsRequest
    {
        public Guid Id { get; set; }

        public string RawValue { get; set; }
    }
}
