using Microsoft.EntityFrameworkCore;

namespace MAS.Payments.DataBase
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        { }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<PaymentType> PaymentTypes { get; set; }

        public DbSet<MeterMeasurement> MeterMeasurements { get; set; }

        public DbSet<MeterMeasurementType> MeterMeasurementTypes { get; set; }

        public DbSet<UserSettings> UserSettings { get; set; }
    }
}