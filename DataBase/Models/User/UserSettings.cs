namespace MAS.Payments.DataBase
{
    public class UserSettings : Entity
    {
        public string MeasurementsEmailToSend { get; set; }

        public bool DisplayMeasurementNotification { get; set; }

        public long UserId { get; set; }

        public virtual User User { get; set; }
    }
}