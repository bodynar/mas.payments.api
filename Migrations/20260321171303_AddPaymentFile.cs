using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MAS.Payments.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentFile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: true),
                    Data = table.Column<byte[]>(type: "bytea", nullable: true),
                    PaymentId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentGroupId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentFile", x => x.Id);
                    table.CheckConstraint("CK_PaymentFile_SingleLink", "(\"PaymentId\" IS NOT NULL AND \"PaymentGroupId\" IS NULL) OR (\"PaymentId\" IS NULL AND \"PaymentGroupId\" IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_PaymentFile_PaymentGroup_PaymentGroupId",
                        column: x => x.PaymentGroupId,
                        principalTable: "PaymentGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentFile_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentFile_PaymentGroupId",
                table: "PaymentFile",
                column: "PaymentGroupId",
                unique: true,
                filter: "\"PaymentGroupId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentFile_PaymentId",
                table: "PaymentFile",
                column: "PaymentId",
                unique: true,
                filter: "\"PaymentId\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentFile");
        }
    }
}
