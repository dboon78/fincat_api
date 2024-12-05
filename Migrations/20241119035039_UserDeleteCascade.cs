using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class UserDeleteCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Currencies_CurrencyCode",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31c14266-6678-460a-8eb5-cb3ad9f9bf72");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6cda119-7ebe-404e-bcd2-93c395d1796b");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "46e1edd6-6ac4-4be1-a2e1-b2808c3948de", null, "User", "USER" },
                    { "b07d610e-5ec5-43fb-af79-c5d057405066", null, "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Currencies_CurrencyCode",
                table: "AspNetUsers",
                column: "CurrencyCode",
                principalTable: "Currencies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Currencies_CurrencyCode",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "46e1edd6-6ac4-4be1-a2e1-b2808c3948de");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b07d610e-5ec5-43fb-af79-c5d057405066");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "31c14266-6678-460a-8eb5-cb3ad9f9bf72", null, "Admin", "ADMIN" },
                    { "f6cda119-7ebe-404e-bcd2-93c395d1796b", null, "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Currencies_CurrencyCode",
                table: "AspNetUsers",
                column: "CurrencyCode",
                principalTable: "Currencies",
                principalColumn: "Code");
        }
    }
}
