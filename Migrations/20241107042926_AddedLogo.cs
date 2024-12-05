using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddedLogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoricPrices_Stocks_StockId",
                table: "HistoricPrices");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ac57e156-4e80-473a-b2b7-ab2ca3ed41c0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f9538a31-eac9-461b-9fd4-f407bd4558af");

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Stocks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4e115166-2d64-47ff-bd45-febbb3399d95", null, "Admin", "ADMIN" },
                    { "e4751182-28cf-4c02-8c94-ed78d15f7944", null, "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricPrices_Stocks_StockId",
                table: "HistoricPrices",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoricPrices_Stocks_StockId",
                table: "HistoricPrices");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e115166-2d64-47ff-bd45-febbb3399d95");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e4751182-28cf-4c02-8c94-ed78d15f7944");

            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Stocks");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "ac57e156-4e80-473a-b2b7-ab2ca3ed41c0", null, "Admin", "ADMIN" },
                    { "f9538a31-eac9-461b-9fd4-f407bd4558af", null, "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricPrices_Stocks_StockId",
                table: "HistoricPrices",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id");
        }
    }
}
