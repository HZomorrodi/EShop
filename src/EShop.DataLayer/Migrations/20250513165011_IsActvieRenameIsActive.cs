using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class IsActvieRenameIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActvie",
                table: "User",
                newName: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "User",
                newName: "IsActvie");
        }
    }
}
