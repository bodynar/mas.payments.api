using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAS.Payments.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedOnToEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "UserSetting",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "UserNotification",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "PaymentType",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "PaymentGroup",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Payment",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "MeterMeasurementType",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "MeterMeasurement",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            // Update existing records with meaningful CreatedOn values

            // MeterMeasurement: 1st day of measurement's month/year
            migrationBuilder.Sql(
                """UPDATE "MeterMeasurement" SET "CreatedOn" = DATE_TRUNC('month', "Date")""");

            // Payment: 1st day of payment date's month/year
            migrationBuilder.Sql(
                """UPDATE "Payment" SET "CreatedOn" = DATE_TRUNC('month', "Date") WHERE "Date" IS NOT NULL""");

            // PaymentGroup: 1st day of group's month/year
            migrationBuilder.Sql(
                """UPDATE "PaymentGroup" SET "CreatedOn" = MAKE_TIMESTAMPTZ("Year", "Month", 1, 0, 0, 0)""");

            // UserNotification: copy from CreatedAt
            migrationBuilder.Sql(
                """UPDATE "UserNotification" SET "CreatedOn" = "CreatedAt" """);

            // PaymentType: 2 July 2019
            migrationBuilder.Sql(
                """UPDATE "PaymentType" SET "CreatedOn" = '2019-07-02T00:00:00Z'""");

            // MeterMeasurementType: 2 July 2019
            migrationBuilder.Sql(
                """UPDATE "MeterMeasurementType" SET "CreatedOn" = '2019-07-02T00:00:00Z'""");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "UserSetting");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "UserNotification");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "PaymentType");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "PaymentGroup");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "MeterMeasurementType");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "MeterMeasurement");
        }
    }
}
