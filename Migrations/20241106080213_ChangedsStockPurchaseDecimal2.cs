using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class ChangedsStockPurchaseDecimal2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3ba967b2-b4a4-453d-9a40-fc593edcdd4d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ad233879-ae5c-496f-a685-98e8537b992c");

            migrationBuilder.AlterColumn<decimal>(
                name: "Purchase",
                table: "Stocks",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "64c44a75-5a21-4d60-9d39-2a8c2f03643a", null, "User", "USER" },
                    { "9295c3b0-0718-4727-842f-304d84bf8f47", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64c44a75-5a21-4d60-9d39-2a8c2f03643a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9295c3b0-0718-4727-842f-304d84bf8f47");

            migrationBuilder.AlterColumn<decimal>(
                name: "Purchase",
                table: "Stocks",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3ba967b2-b4a4-453d-9a40-fc593edcdd4d", null, "User", "USER" },
                    { "ad233879-ae5c-496f-a685-98e8537b992c", null, "Admin", "ADMIN" }
                });
        }
    }
}
