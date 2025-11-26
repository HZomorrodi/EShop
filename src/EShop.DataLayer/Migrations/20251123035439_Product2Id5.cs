using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class Product2Id5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsProductTags_Products_ProductId",
                table: "ProductsProductTags");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductsProductTags",
                newName: "Product2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsProductTags_Products_Product2Id",
                table: "ProductsProductTags",
                column: "Product2Id",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsProductTags_Products_Product2Id",
                table: "ProductsProductTags");

            migrationBuilder.RenameColumn(
                name: "Product2Id",
                table: "ProductsProductTags",
                newName: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsProductTags_Products_ProductId",
                table: "ProductsProductTags",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
