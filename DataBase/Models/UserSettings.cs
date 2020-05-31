namespace MAS.Payments.DataBase
{
    public class UserSettings : Entity
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string RawValue { get; set; }

        public string TypeName { get; set; }
    }
}