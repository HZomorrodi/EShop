using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.DataLayer.Migrations.TicketDb
{
    /// <inheritdoc />
    public partial class UserRoleAdded2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User2s");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Role2s");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User2s",
                table: "User2s",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role2s",
                table: "Role2s",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_User2s",
                table: "User2s");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role2s",
                table: "Role2s");

            migrationBuilder.RenameTable(
                name: "User2s",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Role2s",
                newName: "Roles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");
        }
    }
}
