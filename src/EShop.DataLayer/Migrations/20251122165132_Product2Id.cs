using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class Product2Id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsProductTags2",
                table: "ProductsProductTags2");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsProductTags2",
                table: "ProductsProductTags2",
                columns: new[] { "ProductId", "ProductTagId" });
        }
    }
}
