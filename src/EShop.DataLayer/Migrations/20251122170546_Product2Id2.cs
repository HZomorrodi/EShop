using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class Product2Id2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsProductTags2_ProductTags_ProductTagId",
                table: "ProductsProductTags2");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsProductTags2_Products_ProductId",
                table: "ProductsProductTags2");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsProductTags2",
                table: "ProductsProductTags2");

            migrationBuilder.DropIndex(
                name: "IX_ProductsProductTags2_ProductId",
                table: "ProductsProductTags2");

            migrationBuilder.DropColumn(
                name: "Product2Id",
                table: "ProductsProductTags2");

            migrationBuilder.DropColumn(
                name: "ProductTagId2",
                table: "ProductsProductTags2");

            migrationBuilder.RenameTable(
                name: "ProductsProductTags2",
                newName: "ProductsProductTags");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsProductTags2_ProductTagId",
                table: "ProductsProductTags",
                newName: "IX_ProductsProductTags_ProductTagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsProductTags",
                table: "ProductsProductTags",
                columns: new[] { "ProductId", "ProductTagId" });

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsProductTags",
                table: "ProductsProductTags");

            migrationBuilder.RenameTable(
                name: "ProductsProductTags",
                newName: "ProductsProductTags2");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsProductTags_ProductTagId",
                table: "ProductsProductTags2",
                newName: "IX_ProductsProductTags2_ProductTagId");

            migrationBuilder.AddColumn<int>(
                name: "Product2Id",
                table: "ProductsProductTags2",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductTagId2",
                table: "ProductsProductTags2",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsProductTags2",
                table: "ProductsProductTags2",
                columns: new[] { "Product2Id", "ProductTagId2" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsProductTags2_ProductId",
                table: "ProductsProductTags2",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsProductTags2_ProductTags_ProductTagId",
                table: "ProductsProductTags2",
                column: "ProductTagId",
                principalTable: "ProductTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsProductTags2_Products_ProductId",
                table: "ProductsProductTags2",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
