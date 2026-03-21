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

            migrationBuilder.UpdateData(
                table: "UserSetting",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedOn",
                value: new DateTime(2026, 3, 21, 12, 9, 34, 17, DateTimeKind.Utc).AddTicks(6275));
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
