using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class StocksNeedsNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ce6be36f-2d31-4069-9967-6bf89609db6f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e52ea3d8-d91e-4241-916c-744dd023c5bc");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8d9227f0-b970-490b-a909-a84865817ebc", null, "User", "USER" },
                    { "ff577858-ec5e-4ef5-b27d-0ef83d38649b", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8d9227f0-b970-490b-a909-a84865817ebc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ff577858-ec5e-4ef5-b27d-0ef83d38649b");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "ce6be36f-2d31-4069-9967-6bf89609db6f", null, "User", "USER" },
                    { "e52ea3d8-d91e-4241-916c-744dd023c5bc", null, "Admin", "ADMIN" }
                });
        }
    }
}
