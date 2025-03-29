﻿// <auto-generated />
using System;
using MAS.Payments.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MAS.Payments.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    partial class DataBaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MAS.Payments.DataBase.MeterMeasurement", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double?>("Diff")
                        .HasColumnType("double precision");

                    b.Property<bool>("IsSent")
                        .HasColumnType("boolean");

                    b.Property<double>("Measurement")
                        .HasColumnType("double precision");

                    b.Property<long>("MeterMeasurementTypeId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("MeterMeasurementTypeId");

                    b.ToTable("MeterMeasurement");
                });

            modelBuilder.Entity("MAS.Payments.DataBase.MeterMeasurementType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Color")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<long>("PaymentTypeId")
                        .HasColumnType("bigint");

                    b.Property<string>("SystemName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PaymentTypeId");

                    b.ToTable("MeterMeasurementType");
                });

            modelBuilder.Entity("MAS.Payments.DataBase.Payment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<long>("PaymentTypeId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("PaymentTypeId");

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("MAS.Payments.DataBase.PaymentType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Color")
                        .HasColumnType("text");

                    b.Property<string>("Company")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("SystemName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("PaymentType");
                });

            modelBuilder.Entity("MAS.Payments.DataBase.UserNotification", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("HiddenAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsHidden")
                        .HasColumnType("boolean");

                    b.Property<string>("Key")
                        .HasColumnType("text");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<short>("Type")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.ToTable("UserNotification");
                });

            modelBuilder.Entity("MAS.Payments.DataBase.UserSettings", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("DisplayName")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("RawValue")
                        .HasColumnType("text");

                    b.Property<string>("TypeName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UserSetting");

                    b.HasData(
                        new
                        {
                            Id = 2L,
                            DisplayName = "Отображать уведомления по показаниям",
                            Name = "DisplayMeasurementsNotification",
                            RawValue = "true",
                            TypeName = "Boolean"
                        });
                });

            modelBuilder.Entity("MAS.Payments.DataBase.MeterMeasurement", b =>
                {
                    b.HasOne("MAS.Payments.DataBase.MeterMeasurementType", "MeasurementType")
                        .WithMany("MeterMeasurements")
                        .HasForeignKey("MeterMeasurementTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MeasurementType");
                });

            modelBuilder.Entity("MAS.Payments.DataBase.MeterMeasurementType", b =>
                {
                    b.HasOne("MAS.Payments.DataBase.PaymentType", "PaymentType")
                        .WithMany("MeasurementTypes")
                        .HasForeignKey("PaymentTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PaymentType");
                });

            modelBuilder.Entity("MAS.Payments.DataBase.Payment", b =>
                {
                    b.HasOne("MAS.Payments.DataBase.PaymentType", "PaymentType")
                        .WithMany("Payments")
                        .HasForeignKey("PaymentTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PaymentType");
                });

            modelBuilder.Entity("MAS.Payments.DataBase.MeterMeasurementType", b =>
                {
                    b.Navigation("MeterMeasurements");
                });

            modelBuilder.Entity("MAS.Payments.DataBase.PaymentType", b =>
                {
                    b.Navigation("MeasurementTypes");

                    b.Navigation("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}
