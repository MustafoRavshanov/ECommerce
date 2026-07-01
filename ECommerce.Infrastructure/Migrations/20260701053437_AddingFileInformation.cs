using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingFileInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "file_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    end_point = table.Column<string>(type: "text", nullable: true),
                    file_name = table.Column<string>(type: "text", nullable: true),
                    unique_file_name = table.Column<string>(type: "text", nullable: true),
                    file_length = table.Column<long>(type: "bigint", nullable: false),
                    content_type = table.Column<string>(type: "text", nullable: true),
                    basket_name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_file_data", x => x.id);
                });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "Password_hash" },
                values: new object[] { new DateTime(2026, 7, 1, 5, 34, 36, 472, DateTimeKind.Utc).AddTicks(9580), "$2a$11$/uREhtk0bdQ49Z4PDPAxeu5hoIDjEw/kK8CvgF2C8SoC3zljvnp1W" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "file_data");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "Password_hash" },
                values: new object[] { new DateTime(2026, 6, 26, 5, 2, 33, 275, DateTimeKind.Utc).AddTicks(2041), "$2a$11$qu8hqLuEGj81OlYI9d1CQOofnkh04h6ZphjywmHSSknDyie5XNUG." });
        }
    }
}
