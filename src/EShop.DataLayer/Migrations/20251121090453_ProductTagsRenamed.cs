using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class ProductTagsRenamed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductTag_ProductTag_ProductTagsId",
                table: "ProductProductTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductTag",
                table: "ProductTag");

            migrationBuilder.RenameTable(
                name: "ProductTag",
                newName: "ProductTags");

            migrationBuilder.RenameIndex(
                name: "IX_ProductTag_Title",
                table: "ProductTags",
                newName: "IX_ProductTags_Title");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductTags",
                table: "ProductTags",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductTag_ProductTags_ProductTagsId",
                table: "ProductProductTag",
                column: "ProductTagsId",
                principalTable: "ProductTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductTag_ProductTags_ProductTagsId",
                table: "ProductProductTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductTags",
                table: "ProductTags");

            migrationBuilder.RenameTable(
                name: "ProductTags",
                newName: "ProductTag");

            migrationBuilder.RenameIndex(
                name: "IX_ProductTags_Title",
                table: "ProductTag",
                newName: "IX_ProductTag_Title");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductTag",
                table: "ProductTag",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductTag_ProductTag_ProductTagsId",
                table: "ProductProductTag",
                column: "ProductTagsId",
                principalTable: "ProductTag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
