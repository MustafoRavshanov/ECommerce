using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixingFileSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_url",
                table: "product");

            migrationBuilder.DropColumn(
                name: "file_name",
                table: "file_data");

            migrationBuilder.AddColumn<int>(
                name: "file_data_id",
                table: "product",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "Password_hash" },
                values: new object[] { new DateTime(2026, 7, 1, 10, 5, 11, 109, DateTimeKind.Utc).AddTicks(4177), "$2a$11$HLQ9C61vBSAGOEchLhsrcO6vdtS9z9Y5/d5APItRblMNMGKYhpWMa" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "file_data_id",
                table: "product");

            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "product",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "file_name",
                table: "file_data",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "Password_hash" },
                values: new object[] { new DateTime(2026, 7, 1, 5, 34, 36, 472, DateTimeKind.Utc).AddTicks(9580), "$2a$11$/uREhtk0bdQ49Z4PDPAxeu5hoIDjEw/kK8CvgF2C8SoC3zljvnp1W" });
        }
    }
}
