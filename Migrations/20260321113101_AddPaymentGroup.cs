using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MAS.Payments.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PaymentGroupId",
                table: "Payment",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentGroup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentGroup", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PaymentGroupId",
                table: "Payment",
                column: "PaymentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentGroup_Year_Month",
                table: "PaymentGroup",
                columns: new[] { "Year", "Month" });

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_PaymentGroup_PaymentGroupId",
                table: "Payment",
                column: "PaymentGroupId",
                principalTable: "PaymentGroup",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_PaymentGroup_PaymentGroupId",
                table: "Payment");

            migrationBuilder.DropTable(
                name: "PaymentGroup");

            migrationBuilder.DropIndex(
                name: "IX_Payment_PaymentGroupId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "PaymentGroupId",
                table: "Payment");
        }
    }
}
