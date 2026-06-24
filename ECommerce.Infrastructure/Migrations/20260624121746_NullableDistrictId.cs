using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NullableDistrictId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_customer_district_district_id",
                table: "customer");

            migrationBuilder.AlterColumn<int>(
                name: "district_id",
                table: "customer",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_customer_district_district_id",
                table: "customer",
                column: "district_id",
                principalTable: "district",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_customer_district_district_id",
                table: "customer");

            migrationBuilder.AlterColumn<int>(
                name: "district_id",
                table: "customer",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_customer_district_district_id",
                table: "customer",
                column: "district_id",
                principalTable: "district",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
