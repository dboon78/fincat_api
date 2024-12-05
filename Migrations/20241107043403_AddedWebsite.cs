using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddedWebsite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e115166-2d64-47ff-bd45-febbb3399d95");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e4751182-28cf-4c02-8c94-ed78d15f7944");

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Stocks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0feaabb1-2895-4607-a41e-207035415a74", null, "User", "USER" },
                    { "76eaab19-86db-40d2-845d-280b8ebf3e5d", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0feaabb1-2895-4607-a41e-207035415a74");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "76eaab19-86db-40d2-845d-280b8ebf3e5d");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Stocks");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4e115166-2d64-47ff-bd45-febbb3399d95", null, "Admin", "ADMIN" },
                    { "e4751182-28cf-4c02-8c94-ed78d15f7944", null, "User", "USER" }
                });
        }
    }
}
