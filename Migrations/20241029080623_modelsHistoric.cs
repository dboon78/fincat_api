using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class modelsHistoric : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoricPrices_Stocks_StockId",
                table: "HistoricPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_StockExchanges_ExchangeId",
                table: "Stocks");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2b833449-a9a9-4746-a56f-8e0e6752be99");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "75ff4a0c-8681-404b-9600-c1046299e24c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "691a0338-f257-46e4-9c9f-52bf2a40afa8", null, "User", "USER" },
                    { "a8bb0747-06c1-4a28-a266-075e0b7f393f", null, "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricPrices_Stocks_StockId",
                table: "HistoricPrices",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_StockExchanges_ExchangeId",
                table: "Stocks",
                column: "ExchangeId",
                principalTable: "StockExchanges",
                principalColumn: "ExchangeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoricPrices_Stocks_StockId",
                table: "HistoricPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_StockExchanges_ExchangeId",
                table: "Stocks");

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
                    { "2b833449-a9a9-4746-a56f-8e0e6752be99", null, "Admin", "ADMIN" },
                    { "75ff4a0c-8681-404b-9600-c1046299e24c", null, "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricPrices_Stocks_StockId",
                table: "HistoricPrices",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_StockExchanges_ExchangeId",
                table: "Stocks",
                column: "ExchangeId",
                principalTable: "StockExchanges",
                principalColumn: "ExchangeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
