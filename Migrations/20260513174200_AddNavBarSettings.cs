using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MAS.Payments.Migrations
{
    /// <inheritdoc />
    public partial class AddNavBarSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserSetting",
                columns: new[] { "Id", "CreatedOn", "DisplayName", "Name", "RawValue", "TypeName" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Цвет панели навигации", "NavBarColor", "#363636", "Color" },
                    { new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Название сайта", "NavBarTitle", "Payments", "String" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserSetting",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"));

            migrationBuilder.DeleteData(
                table: "UserSetting",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"));
        }
    }
}
