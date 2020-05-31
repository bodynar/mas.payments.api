using System.ComponentModel;

namespace MAS.Payments.DataBase
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1717:Only FlagsAttribute enums should have plural names", Justification = "<Pending>")]
    public enum DefaultUserSettings
    {
        /// <summary>
        /// E-mail для отправки показаний
        /// </summary>
        [Description("E-mail для отправки показаний")]
        EmailToSendMeasurements = 1,

        /// <summary>
        /// Отображать уведомления по показаниям
        /// </summary>
        [Description("Отображать уведомления по показаниям")]
        DisplayMeasurementsNotification = 2,
    }
}