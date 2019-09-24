using Microsoft.EntityFrameworkCore.Migrations;

namespace MAS.Payments.Migrations
{
    public partial class add_author_relation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "PaymentTypes",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentTypes_UserId",
                table: "PaymentTypes",
                newName: "IX_PaymentTypes_AuthorId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Payments",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                newName: "IX_Payments_AuthorId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "MeterMeasurementTypes",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_MeterMeasurementTypes_UserId",
                table: "MeterMeasurementTypes",
                newName: "IX_MeterMeasurementTypes_AuthorId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "MeterMeasurements",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_MeterMeasurements_UserId",
                table: "MeterMeasurements",
                newName: "IX_MeterMeasurements_AuthorId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "PaymentTypes",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentTypes_AuthorId",
                table: "PaymentTypes",
                newName: "IX_PaymentTypes_UserId");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Payments",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_AuthorId",
                table: "Payments",
                newName: "IX_Payments_UserId");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "MeterMeasurementTypes",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MeterMeasurementTypes_AuthorId",
                table: "MeterMeasurementTypes",
                newName: "IX_MeterMeasurementTypes_UserId");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "MeterMeasurements",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MeterMeasurements_AuthorId",
                table: "MeterMeasurements",
                newName: "IX_MeterMeasurements_UserId");

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
    }
}
