using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class Product2Id8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsProductTags_ProductTags_ProductTagsId",
                table: "ProductsProductTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsProductTags_Products_ProductsId",
                table: "ProductsProductTags");

            migrationBuilder.RenameColumn(
                name: "ProductsId",
                table: "ProductsProductTags",
                newName: "ProductTagId");

            migrationBuilder.RenameColumn(
                name: "ProductTagsId",
                table: "ProductsProductTags",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsProductTags_ProductsId",
                table: "ProductsProductTags",
                newName: "IX_ProductsProductTags_ProductTagId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsProductTags_ProductTags_ProductTagId",
                table: "ProductsProductTags",
                column: "ProductTagId",
                principalTable: "ProductTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsProductTags_Products_ProductId",
                table: "ProductsProductTags",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsProductTags_ProductTags_ProductTagId",
                table: "ProductsProductTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsProductTags_Products_ProductId",
                table: "ProductsProductTags");

            migrationBuilder.RenameColumn(
                name: "ProductTagId",
                table: "ProductsProductTags",
                newName: "ProductsId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductsProductTags",
                newName: "ProductTagsId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsProductTags_ProductTagId",
                table: "ProductsProductTags",
                newName: "IX_ProductsProductTags_ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsProductTags_ProductTags_ProductTagsId",
                table: "ProductsProductTags",
                column: "ProductTagsId",
                principalTable: "ProductTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsProductTags_Products_ProductsId",
                table: "ProductsProductTags",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
