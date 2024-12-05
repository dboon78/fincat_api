using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyAndUserChanges3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "37c48024-d4d9-46b3-a1e3-ee4857b98cb5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3354691-4067-4391-9c28-6dd8c8727f78");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "31c14266-6678-460a-8eb5-cb3ad9f9bf72", null, "Admin", "ADMIN" },
                    { "f6cda119-7ebe-404e-bcd2-93c395d1796b", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                    { "37c48024-d4d9-46b3-a1e3-ee4857b98cb5", null, "User", "USER" },
                    { "b3354691-4067-4391-9c28-6dd8c8727f78", null, "Admin", "ADMIN" }
                });
        }
    }
}
