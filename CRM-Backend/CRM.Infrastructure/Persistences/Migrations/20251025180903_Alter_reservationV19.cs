using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Infrastructure.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class Alter_reservationV19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCatalog_Catalog_CatalogoId",
                table: "SubCatalog");

            migrationBuilder.RenameColumn(
                name: "CatalogoId",
                table: "SubCatalog",
                newName: "CatalogId");

            migrationBuilder.RenameIndex(
                name: "IX_SubCatalog_CatalogoId",
                table: "SubCatalog",
                newName: "IX_SubCatalog_CatalogId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCatalog_Catalog_CatalogId",
                table: "SubCatalog",
                column: "CatalogId",
                principalTable: "Catalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCatalog_Catalog_CatalogId",
                table: "SubCatalog");

            migrationBuilder.RenameColumn(
                name: "CatalogId",
                table: "SubCatalog",
                newName: "CatalogoId");

            migrationBuilder.RenameIndex(
                name: "IX_SubCatalog_CatalogId",
                table: "SubCatalog",
                newName: "IX_SubCatalog_CatalogoId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCatalog_Catalog_CatalogoId",
                table: "SubCatalog",
                column: "CatalogoId",
                principalTable: "Catalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
