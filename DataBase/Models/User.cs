using System.Collections.Generic;

namespace MAS.Payments.DataBase
{
    public class User : Entity
    {
        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsActive { get; set; }

        public virtual UserSettings UserSettings { get; set; }

        public virtual IEnumerable<MeterMeasurement> Measurements { get; set; }

        public virtual ICollection<MeterMeasurementType> MeasurementTypes { get; set; }

        public virtual IEnumerable<Payment> Payments { get; set; }

        public virtual IEnumerable<PaymentType> PaymentTypes { get; set; }

        public User()
        {
            Measurements = new List<MeterMeasurement>();
            MeasurementTypes = new List<MeterMeasurementType>();
            Payments = new List<Payment>();
            PaymentTypes = new List<PaymentType>();
        }
    }
}