using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class ProductProductTagWithoutId : Migration
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
                name: "Id",
                table: "ProductsProductTags2");

            migrationBuilder.RenameTable(
                name: "ProductsProductTags2",
                newName: "ProductProductTag");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsProductTags2_ProductTagId",
                table: "ProductProductTag",
                newName: "IX_ProductProductTag_ProductTagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductProductTag",
                table: "ProductProductTag",
                columns: new[] { "ProductId", "ProductTagId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductTag_ProductTags_ProductTagId",
                table: "ProductProductTag",
                column: "ProductTagId",
                principalTable: "ProductTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductTag_Products_ProductId",
                table: "ProductProductTag",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductTag_ProductTags_ProductTagId",
                table: "ProductProductTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductTag_Products_ProductId",
                table: "ProductProductTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductProductTag",
                table: "ProductProductTag");

            migrationBuilder.RenameTable(
                name: "ProductProductTag",
                newName: "ProductsProductTags2");

            migrationBuilder.RenameIndex(
                name: "IX_ProductProductTag_ProductTagId",
                table: "ProductsProductTags2",
                newName: "IX_ProductsProductTags2_ProductTagId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ProductsProductTags2",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsProductTags2",
                table: "ProductsProductTags2",
                column: "Id");

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
