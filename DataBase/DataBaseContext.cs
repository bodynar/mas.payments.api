namespace MAS.Payments.DataBase
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

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

        public DbSet<PaymentGroup> PaymentGroup { get; set; }

        public DbSet<PaymentFile> PaymentFile { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(Entity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(Entity.Id))
                        .ValueGeneratedNever();
                }
            }

            // todo: use DefaultUserSetting enum and its attribute
            modelBuilder
                .Entity<UserSettings>()
                .HasData(
                    new UserSettings { Id = Guid.Parse("d3f5a7b2-1e4c-4f8a-9b6d-2c7e8f0a1b3d"), DisplayName = "Отображать уведомления по показаниям", Name = "DisplayMeasurementsNotification", TypeName = SettingDataValueType.Boolean.ToString(), RawValue = true.ToString().ToLower() }
                );

            modelBuilder.Entity<PaymentGroup>()
                .HasIndex(x => new { x.Year, x.Month });

            modelBuilder.Entity<PaymentFile>(entity =>
            {
                entity.HasOne(f => f.Payment)
                    .WithOne()
                    .HasForeignKey<PaymentFile>(f => f.PaymentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(f => f.PaymentGroup)
                    .WithOne()
                    .HasForeignKey<PaymentFile>(f => f.PaymentGroupId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(f => f.PaymentId)
                    .IsUnique()
                    .HasFilter("\"PaymentId\" IS NOT NULL");

                entity.HasIndex(f => f.PaymentGroupId)
                    .IsUnique()
                    .HasFilter("\"PaymentGroupId\" IS NOT NULL");

                entity.ToTable(t => t.HasCheckConstraint(
                    "CK_PaymentFile_SingleLink",
                    "(\"PaymentId\" IS NOT NULL AND \"PaymentGroupId\" IS NULL) OR (\"PaymentId\" IS NULL AND \"PaymentGroupId\" IS NOT NULL)"
                ));
            });

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<Entity>().Where(e => e.State == EntityState.Added))
            {
                if (entry.Entity.Id == default)
                {
                    entry.Entity.Id = Guid.NewGuid();
                }

                if (entry.Entity.CreatedOn == default)
                {
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
