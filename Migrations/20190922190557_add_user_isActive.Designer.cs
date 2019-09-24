﻿// <auto-generated />
using System;
using MAS.Payments.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MAS.Payments.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    [Migration("20190922190557_add_user_isActive")]
    partial class add_user_isActive
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MAS.Payments.DataBase.MeterMeasurement", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment");

                    b.Property<DateTime>("Date");

                    b.Property<bool>("IsSent");

                    b.Property<double>("Measurement");

                    b.Property<long>("MeterMeasurementTypeId");

                    b.Property<long?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("MeterMeasurementTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("MeterMeasurements");
                });

            modelBuilder.Entity("MAS.Payments.DataBase.MeterMeasurementType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<long>("PaymentTypeId");

                    b.Property<string>("SystemName");

                    b.Property<long?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PaymentTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("MeterMeasurementTypes");
                });

            modelBuilder.Entity("MAS.Payments.DataBase.Payment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount");

                    b.Property<DateTime?>("Date");

                    b.Property<string>("Description");

                    b.Property<long>("PaymentTypeId");

                    b.Property<long?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PaymentTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("MAS.Payments.DataBase.PaymentType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Company");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("SystemName");

                    b.Property<long?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("PaymentTypes");
                });

            modelBuilder.Entity("MAS.Payments.DataBase.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsActive");

                    b.Property<string>("LastName");

                    b.Property<string>("Login");

                    b.Property<string>("PasswordHash");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MAS.Payments.DataBase.UserSettings", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("DisplayMeasurementNotification");

                    b.Property<string>("MeasurementsEmailToSend");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserSettings");
                });

            modelBuilder.Entity("MAS.Payments.DataBase.MeterMeasurement", b =>
                {
                    b.HasOne("MAS.Payments.DataBase.MeterMeasurementType", "MeasurementType")
                        .WithMany("MeterMeasurements")
                        .HasForeignKey("MeterMeasurementTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MAS.Payments.DataBase.User")
                        .WithMany("Measurements")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("MAS.Payments.DataBase.MeterMeasurementType", b =>
                {
                    b.HasOne("MAS.Payments.DataBase.PaymentType", "PaymentType")
                        .WithMany("MeasurementTypes")
                        .HasForeignKey("PaymentTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MAS.Payments.DataBase.User")
                        .WithMany("MeasurementTypes")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("MAS.Payments.DataBase.Payment", b =>
                {
                    b.HasOne("MAS.Payments.DataBase.PaymentType", "PaymentType")
                        .WithMany("Payments")
                        .HasForeignKey("PaymentTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MAS.Payments.DataBase.User")
                        .WithMany("Payments")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("MAS.Payments.DataBase.PaymentType", b =>
                {
                    b.HasOne("MAS.Payments.DataBase.User")
                        .WithMany("PaymentTypes")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("MAS.Payments.DataBase.UserSettings", b =>
                {
                    b.HasOne("MAS.Payments.DataBase.User", "User")
                        .WithOne("UserSettings")
                        .HasForeignKey("MAS.Payments.DataBase.UserSettings", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}