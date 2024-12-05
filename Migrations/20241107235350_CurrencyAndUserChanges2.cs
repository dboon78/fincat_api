using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyAndUserChanges2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "188d5b2a-21e3-4839-a3b0-d769b3fe1734");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "90a0c4dd-de6b-4412-96e7-9e1915fe8c53");

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyCode",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true,
                defaultValue: "USD",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "37c48024-d4d9-46b3-a1e3-ee4857b98cb5", null, "User", "USER" },
                    { "b3354691-4067-4391-9c28-6dd8c8727f78", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "37c48024-d4d9-46b3-a1e3-ee4857b98cb5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3354691-4067-4391-9c28-6dd8c8727f78");

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyCode",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true,
                oldDefaultValue: "USD");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "188d5b2a-21e3-4839-a3b0-d769b3fe1734", null, "Admin", "ADMIN" },
                    { "90a0c4dd-de6b-4412-96e7-9e1915fe8c53", null, "User", "USER" }
                });
        }
    }
}
