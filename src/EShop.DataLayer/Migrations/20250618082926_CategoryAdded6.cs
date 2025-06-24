using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class CategoryAdded6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title2",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title2",
                table: "Categories");
        }
    }
}
