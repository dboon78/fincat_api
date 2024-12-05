using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyAndUserChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_StockExchanges_ExchangeId",
                table: "Stocks");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8d9227f0-b970-490b-a909-a84865817ebc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ff577858-ec5e-4ef5-b27d-0ef83d38649b");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Digits = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Code);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "188d5b2a-21e3-4839-a3b0-d769b3fe1734", null, "Admin", "ADMIN" },
                    { "90a0c4dd-de6b-4412-96e7-9e1915fe8c53", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CurrencyCode",
                table: "AspNetUsers",
                column: "CurrencyCode");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Currencies_CurrencyCode",
                table: "AspNetUsers",
                column: "CurrencyCode",
                principalTable: "Currencies",
                principalColumn: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_StockExchange_ExchangeId",
                table: "Stocks",
                column: "ExchangeId",
                principalTable: "StockExchanges",
                principalColumn: "ExchangeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Currencies_CurrencyCode",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_StockExchange_ExchangeId",
                table: "Stocks");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CurrencyCode",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "188d5b2a-21e3-4839-a3b0-d769b3fe1734");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "90a0c4dd-de6b-4412-96e7-9e1915fe8c53");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8d9227f0-b970-490b-a909-a84865817ebc", null, "User", "USER" },
                    { "ff577858-ec5e-4ef5-b27d-0ef83d38649b", null, "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_StockExchanges_ExchangeId",
                table: "Stocks",
                column: "ExchangeId",
                principalTable: "StockExchanges",
                principalColumn: "ExchangeId");
        }
    }
}
