using Microsoft.EntityFrameworkCore.Migrations;

namespace MAS.Payments.Migrations
{
    public partial class updateTypeAddSystemName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SystemName",
                table: "PaymentTypes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SystemName",
                table: "MeterMeasurementTypes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SystemName",
                table: "PaymentTypes");

            migrationBuilder.DropColumn(
                name: "SystemName",
                table: "MeterMeasurementTypes");
        }
    }
}
