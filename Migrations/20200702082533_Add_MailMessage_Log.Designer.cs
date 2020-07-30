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
    [Migration("20200702082533_Add_MailMessage_Log")]
    partial class Add_MailMessage_Log
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MAS.Payments.DataBase.MailMessageLogItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body");

                    b.Property<string>("Recipient");

                    b.Property<DateTime>("SentDate");

                    b.Property<string>("Subject");

                    b.HasKey("Id");

                    b.ToTable("MailMessageLog");
                });

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

                    b.HasKey("Id");

                    b.HasIndex("MeterMeasurementTypeId");

                    b.ToTable("MeterMeasurement");
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

                    b.HasKey("Id");

                    b.HasIndex("PaymentTypeId");

                    b.ToTable("MeterMeasurementType");
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

                    b.HasKey("Id");

                    b.HasIndex("PaymentTypeId");

                    b.ToTable("Payment");
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

                    b.HasKey("Id");

                    b.ToTable("PaymentType");
                });

            modelBuilder.Entity("MAS.Payments.DataBase.UserNotification", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime?>("HiddenAt");

                    b.Property<bool>("IsHidden");

                    b.Property<string>("Key");

                    b.Property<string>("Text");

                    b.Property<string>("Title");

                    b.Property<short>("Type");

                    b.HasKey("Id");

                    b.ToTable("UserNotification");
                });

            modelBuilder.Entity("MAS.Payments.DataBase.UserSettings", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DisplayName");

                    b.Property<string>("Name");

                    b.Property<string>("RawValue");

                    b.Property<string>("TypeName");

                    b.HasKey("Id");

                    b.ToTable("UserSetting");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            DisplayName = "E-mail для отправки показаний",
                            Name = "EmailToSendMeasurements",
                            RawValue = "",
                            TypeName = "String"
                        },
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
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MAS.Payments.DataBase.MeterMeasurementType", b =>
                {
                    b.HasOne("MAS.Payments.DataBase.PaymentType", "PaymentType")
                        .WithMany("MeasurementTypes")
                        .HasForeignKey("PaymentTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MAS.Payments.DataBase.Payment", b =>
                {
                    b.HasOne("MAS.Payments.DataBase.PaymentType", "PaymentType")
                        .WithMany("Payments")
                        .HasForeignKey("PaymentTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}