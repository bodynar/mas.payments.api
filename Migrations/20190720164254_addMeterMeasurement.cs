using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MAS.Payments.Migrations
{
    public partial class addMeterMeasurement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeterMeasurementTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PaymentTypeId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterMeasurementTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeterMeasurementTypes_PaymentTypes_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "PaymentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MeterMeasurements",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Measurement = table.Column<double>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    MeterMeasurementTypeId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterMeasurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeterMeasurements_MeterMeasurementTypes_MeterMeasurementTypeId",
                        column: x => x.MeterMeasurementTypeId,
                        principalTable: "MeterMeasurementTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeterMeasurements_MeterMeasurementTypeId",
                table: "MeterMeasurements",
                column: "MeterMeasurementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MeterMeasurementTypes_PaymentTypeId",
                table: "MeterMeasurementTypes",
                column: "PaymentTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeterMeasurements");

            migrationBuilder.DropTable(
                name: "MeterMeasurementTypes");
        }
    }
}
