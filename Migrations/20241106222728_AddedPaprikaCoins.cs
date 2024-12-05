using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddedPaprikaCoins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64c44a75-5a21-4d60-9d39-2a8c2f03643a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9295c3b0-0718-4727-842f-304d84bf8f47");

            migrationBuilder.CreateTable(
                name: "CoinPaprikaCoins",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    rank = table.Column<int>(type: "int", nullable: false),
                    is_new = table.Column<bool>(type: "bit", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinPaprikaCoins", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "ac57e156-4e80-473a-b2b7-ab2ca3ed41c0", null, "Admin", "ADMIN" },
                    { "f9538a31-eac9-461b-9fd4-f407bd4558af", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoinPaprikaCoins");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ac57e156-4e80-473a-b2b7-ab2ca3ed41c0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f9538a31-eac9-461b-9fd4-f407bd4558af");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "64c44a75-5a21-4d60-9d39-2a8c2f03643a", null, "User", "USER" },
                    { "9295c3b0-0718-4727-842f-304d84bf8f47", null, "Admin", "ADMIN" }
                });
        }
    }
}
