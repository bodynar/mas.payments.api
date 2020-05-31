namespace MAS.Payments.Queries
{
    public class GetUserSettingsQueryResult
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string TypeName { get; set; }

        public string RawValue { get; set; }
    }
}