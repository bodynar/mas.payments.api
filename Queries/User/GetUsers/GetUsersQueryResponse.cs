namespace MAS.Payments.Queries
{
    public class GetUsersQueryResponse
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public UserSettings Settings { get; set; }

        public class UserSettings
        {
            public string MeasurementsEmailToSend { get; set; }

            public bool DisplayMeasurementNotification { get; set; }
        }
    }
}