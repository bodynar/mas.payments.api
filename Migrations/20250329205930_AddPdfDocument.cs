using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MAS.Payments.Migrations
{
    /// <inheritdoc />
    public partial class AddPdfDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CheckId",
                table: "Payment",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ReceiptId",
                table: "Payment",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PdfDocuments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    FileData = table.Column<byte[]>(type: "bytea", nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PdfDocuments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_CheckId",
                table: "Payment",
                column: "CheckId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_ReceiptId",
                table: "Payment",
                column: "ReceiptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_PdfDocuments_CheckId",
                table: "Payment",
                column: "CheckId",
                principalTable: "PdfDocuments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_PdfDocuments_ReceiptId",
                table: "Payment",
                column: "ReceiptId",
                principalTable: "PdfDocuments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_PdfDocuments_CheckId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_PdfDocuments_ReceiptId",
                table: "Payment");

            migrationBuilder.DropTable(
                name: "PdfDocuments");

            migrationBuilder.DropIndex(
                name: "IX_Payment_CheckId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_ReceiptId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "CheckId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "ReceiptId",
                table: "Payment");
        }
    }
}
