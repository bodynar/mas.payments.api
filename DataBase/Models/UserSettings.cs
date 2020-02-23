namespace MAS.Payments.DataBase
{
    public class UserSettings : Entity
    {
        public string MeasurementsEmailToSend { get; set; }

        public bool DisplayMeasurementNotification { get; set; }
    }
}