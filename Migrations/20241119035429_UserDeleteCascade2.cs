using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class UserDeleteCascade2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    { "253eb1ec-bc06-4dc6-9995-a03200e77b9a", null, "User", "USER" },
                    { "a5b6f238-3be0-4bd4-b877-2e3cca2f69a5", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "253eb1ec-bc06-4dc6-9995-a03200e77b9a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a5b6f238-3be0-4bd4-b877-2e3cca2f69a5");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "46e1edd6-6ac4-4be1-a2e1-b2808c3948de", null, "User", "USER" },
                    { "b07d610e-5ec5-43fb-af79-c5d057405066", null, "Admin", "ADMIN" }
                });
        }
    }
}
