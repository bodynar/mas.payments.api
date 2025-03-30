namespace MAS.Payments.DataBase
{
    using Microsoft.EntityFrameworkCore;

    public class DataBaseContext(
        DbContextOptions<DataBaseContext> options
    ) : DbContext(options)
    {
        public DbSet<Payment> Payment { get; set; }

        public DbSet<PaymentType> PaymentType { get; set; }

        public DbSet<MeterMeasurement> MeterMeasurement { get; set; }

        public DbSet<MeterMeasurementType> MeterMeasurementType { get; set; }

        public DbSet<UserSettings> UserSetting { get; set; }

        public DbSet<UserNotification> UserNotification { get; set; }

        public DbSet<PdfDocument> PdfDocuments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // todo: use DefaultUserSetting enum and its attribute
            modelBuilder
                .Entity<UserSettings>()
                .HasData(
                    new UserSettings { Id = 2, DisplayName = "Отображать уведомления по показаниям", Name = "DisplayMeasurementsNotification", TypeName = SettingDataValueType.Boolean.ToString(), RawValue = true.ToString().ToLower() }
                );

            modelBuilder.Entity<PdfDocument>()
                .Property(d => d.FileData)
                .HasColumnType("bytea");

            base.OnModelCreating(modelBuilder);
        }
    }
}