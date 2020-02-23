using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MAS.Payments.Migrations
{
    public partial class removeUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeterMeasurements_Users_AuthorId",
                table: "MeterMeasurements");

            migrationBuilder.DropForeignKey(
                name: "FK_MeterMeasurementTypes_Users_AuthorId",
                table: "MeterMeasurementTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_AuthorId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTypes_Users_AuthorId",
                table: "PaymentTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSettings_Users_UserId",
                table: "UserSettings");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserSettings_UserId",
                table: "UserSettings");

            migrationBuilder.DropIndex(
                name: "IX_PaymentTypes_AuthorId",
                table: "PaymentTypes");

            migrationBuilder.DropIndex(
                name: "IX_Payments_AuthorId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_MeterMeasurementTypes_AuthorId",
                table: "MeterMeasurementTypes");

            migrationBuilder.DropIndex(
                name: "IX_MeterMeasurements_AuthorId",
                table: "MeterMeasurements");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "PaymentTypes");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "MeterMeasurementTypes");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "MeterMeasurements");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "UserSettings",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "AuthorId",
                table: "PaymentTypes",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuthorId",
                table: "Payments",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuthorId",
                table: "MeterMeasurementTypes",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuthorId",
                table: "MeterMeasurements",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    Login = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_UserId",
                table: "UserSettings",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTypes_AuthorId",
                table: "PaymentTypes",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AuthorId",
                table: "Payments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_MeterMeasurementTypes_AuthorId",
                table: "MeterMeasurementTypes",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_MeterMeasurements_AuthorId",
                table: "MeterMeasurements",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeterMeasurements_Users_AuthorId",
                table: "MeterMeasurements",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MeterMeasurementTypes_Users_AuthorId",
                table: "MeterMeasurementTypes",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_AuthorId",
                table: "Payments",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTypes_Users_AuthorId",
                table: "PaymentTypes",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSettings_Users_UserId",
                table: "UserSettings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
