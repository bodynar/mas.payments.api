using Microsoft.EntityFrameworkCore.Migrations;

namespace MAS.Payments.Migrations
{
    public partial class AddColorToTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "PaymentType",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "MeterMeasurementType",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "PaymentType");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "MeterMeasurementType");
        }
    }
}
