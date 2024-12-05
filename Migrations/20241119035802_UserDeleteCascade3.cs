using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class UserDeleteCascade3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Holdings_Portfolios_PortfolioId",
                table: "Holdings");

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
                    { "5537c183-aeaa-4064-a2f6-9712971bddba", null, "Admin", "ADMIN" },
                    { "cd1bc6a4-5d84-48cf-be2c-33616fe08245", null, "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Holdings_Portfolios_PortfolioId",
                table: "Holdings",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "PortfolioId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Holdings_Portfolios_PortfolioId",
                table: "Holdings");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5537c183-aeaa-4064-a2f6-9712971bddba");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cd1bc6a4-5d84-48cf-be2c-33616fe08245");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "253eb1ec-bc06-4dc6-9995-a03200e77b9a", null, "User", "USER" },
                    { "a5b6f238-3be0-4bd4-b877-2e3cca2f69a5", null, "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Holdings_Portfolios_PortfolioId",
                table: "Holdings",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "PortfolioId");
        }
    }
}
