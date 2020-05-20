using Microsoft.EntityFrameworkCore;

namespace MAS.Payments.DataBase
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        { }

        public DbSet<Payment> Payment { get; set; }

        public DbSet<PaymentType> PaymentType { get; set; }

        public DbSet<MeterMeasurement> MeterMeasurement { get; set; }

        public DbSet<MeterMeasurementType> MeterMeasurementType { get; set; }

        public DbSet<UserSettings> UserSetting { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<UserSettings>()
                .HasData(
                    new UserSettings { Id = 1, Name = "EmailToSendMeasurements", TypeName = SettingDataValueType.String.ToString(), RawValue = string.Empty },
                    new UserSettings { Id = 2, Name = "DisplayMeasurementsNotification", TypeName = SettingDataValueType.Boolean.ToString(), RawValue = true.ToString() }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}