using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class Product2Id4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsProductTags",
                table: "ProductsProductTags");

            migrationBuilder.DropIndex(
                name: "IX_ProductsProductTags_ProductId",
                table: "ProductsProductTags");

            migrationBuilder.DropColumn(
                name: "ProductzId",
                table: "ProductsProductTags");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsProductTags",
                table: "ProductsProductTags",
                columns: new[] { "ProductId", "ProductTagId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsProductTags",
                table: "ProductsProductTags");

            migrationBuilder.AddColumn<int>(
                name: "ProductzId",
                table: "ProductsProductTags",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsProductTags",
                table: "ProductsProductTags",
                columns: new[] { "ProductzId", "ProductTagId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsProductTags_ProductId",
                table: "ProductsProductTags",
                column: "ProductId");
        }
    }
}
