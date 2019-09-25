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

        public DbSet<User> Users { get; set; }

        public DbSet<UserSettings> UserSettings { get; set; }

        public DbSet<UserToken> UserTokens { get; set; }

        public DbSet<UserTokenType> UserTokenTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<UserTokenType>()
                .HasData(
                    new UserTokenType { Id = 1, Name = "Auth", Description = "Token used for authentication system" },
                    new UserTokenType { Id = 2, Name = "RegistrationConfirm", Description = "Token used for user registration" }
                );
        }
    }
}