using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MAS.Payments.Migrations
{
    public partial class add_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "PaymentTypes",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Payments",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "MeterMeasurementTypes",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "MeterMeasurements",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Login = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MeasurementsEmailToSend = table.Column<string>(nullable: true),
                    DisplayMeasurementNotification = table.Column<bool>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSettings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTypes_UserId",
                table: "PaymentTypes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MeterMeasurementTypes_UserId",
                table: "MeterMeasurementTypes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MeterMeasurements_UserId",
                table: "MeterMeasurements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_UserId",
                table: "UserSettings",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MeterMeasurements_Users_UserId",
                table: "MeterMeasurements",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MeterMeasurementTypes_Users_UserId",
                table: "MeterMeasurementTypes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_UserId",
                table: "Payments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTypes_Users_UserId",
                table: "PaymentTypes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeterMeasurements_Users_UserId",
                table: "MeterMeasurements");

            migrationBuilder.DropForeignKey(
                name: "FK_MeterMeasurementTypes_Users_UserId",
                table: "MeterMeasurementTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_UserId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTypes_Users_UserId",
                table: "PaymentTypes");

            migrationBuilder.DropTable(
                name: "UserSettings");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_PaymentTypes_UserId",
                table: "PaymentTypes");

            migrationBuilder.DropIndex(
                name: "IX_Payments_UserId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_MeterMeasurementTypes_UserId",
                table: "MeterMeasurementTypes");

            migrationBuilder.DropIndex(
                name: "IX_MeterMeasurements_UserId",
                table: "MeterMeasurements");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PaymentTypes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MeterMeasurementTypes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MeterMeasurements");
        }
    }
}
