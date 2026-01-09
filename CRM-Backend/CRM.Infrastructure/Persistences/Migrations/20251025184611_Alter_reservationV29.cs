using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Infrastructure.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class Alter_reservationV29 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubCatalogId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubCatlogId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_SubCatalogId",
                table: "Reservations",
                column: "SubCatalogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_SubCatalog_SubCatalogId",
                table: "Reservations",
                column: "SubCatalogId",
                principalTable: "SubCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_SubCatalog_SubCatalogId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_SubCatalogId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "SubCatalogId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "SubCatlogId",
                table: "Reservations");
        }
    }
}
