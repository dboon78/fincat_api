using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class ChangedsStockPurchaseDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "691a0338-f257-46e4-9c9f-52bf2a40afa8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a8bb0747-06c1-4a28-a266-075e0b7f393f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3ba967b2-b4a4-453d-9a40-fc593edcdd4d", null, "User", "USER" },
                    { "ad233879-ae5c-496f-a685-98e8537b992c", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3ba967b2-b4a4-453d-9a40-fc593edcdd4d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ad233879-ae5c-496f-a685-98e8537b992c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "691a0338-f257-46e4-9c9f-52bf2a40afa8", null, "User", "USER" },
                    { "a8bb0747-06c1-4a28-a266-075e0b7f393f", null, "Admin", "ADMIN" }
                });
        }
    }
}
