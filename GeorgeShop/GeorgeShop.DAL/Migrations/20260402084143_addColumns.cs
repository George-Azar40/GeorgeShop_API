using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeorgeShop.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BrandImage",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrandImage",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Brands");
        }
    }
}
