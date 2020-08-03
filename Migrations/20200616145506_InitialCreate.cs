using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MAS.Payments.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentType",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SystemName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_PaymentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSetting",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    RawValue = table.Column<string>(nullable: true),
                    TypeName = table.Column<string>(nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_UserSetting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeterMeasurementType",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SystemName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PaymentTypeId = table.Column<long>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_MeterMeasurementType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeterMeasurementType_PaymentType_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "PaymentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<double>(nullable: false),
                    Date = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PaymentTypeId = table.Column<long>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payment_PaymentType_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "PaymentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MeterMeasurement",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Measurement = table.Column<double>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    IsSent = table.Column<bool>(nullable: false),
                    MeterMeasurementTypeId = table.Column<long>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_MeterMeasurement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeterMeasurement_MeterMeasurementType_MeterMeasurementTypeId",
                        column: x => x.MeterMeasurementTypeId,
                        principalTable: "MeterMeasurementType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "UserSetting",
                columns: new[] { "Id", "DisplayName", "Name", "RawValue", "TypeName" },
                values: new object[] { 1L, "E-mail для отправки показаний", "EmailToSendMeasurements", "", "String" });

            migrationBuilder.InsertData(
                table: "UserSetting",
                columns: new[] { "Id", "DisplayName", "Name", "RawValue", "TypeName" },
                values: new object[] { 2L, "Отображать уведомления по показаниям", "DisplayMeasurementsNotification", "true", "Boolean" });

            migrationBuilder.CreateIndex(
                name: "IX_MeterMeasurement_MeterMeasurementTypeId",
                table: "MeterMeasurement",
                column: "MeterMeasurementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MeterMeasurementType_PaymentTypeId",
                table: "MeterMeasurementType",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PaymentTypeId",
                table: "Payment",
                column: "PaymentTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeterMeasurement");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "UserSetting");

            migrationBuilder.DropTable(
                name: "MeterMeasurementType");

            migrationBuilder.DropTable(
                name: "PaymentType");
        }
    }
}
