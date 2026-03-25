using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAS.Payments.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentGroupTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentGroupTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentGroupTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentGroupTemplateItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentGroupTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentGroupTemplateItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentGroupTemplateItem_PaymentGroupTemplate_PaymentGroupT~",
                        column: x => x.PaymentGroupTemplateId,
                        principalTable: "PaymentGroupTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentGroupTemplateItem_PaymentType_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "PaymentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentGroupTemplateItem_PaymentGroupTemplateId",
                table: "PaymentGroupTemplateItem",
                column: "PaymentGroupTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentGroupTemplateItem_PaymentTypeId",
                table: "PaymentGroupTemplateItem",
                column: "PaymentTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentGroupTemplateItem");

            migrationBuilder.DropTable(
                name: "PaymentGroupTemplate");
        }
    }
}
