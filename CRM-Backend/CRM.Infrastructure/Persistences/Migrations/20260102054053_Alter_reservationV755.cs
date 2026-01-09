using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Infrastructure.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class Alter_reservationV755 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "price",
                table: "SubCatalog",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "SubCatalog",
                newName: "Code");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "Duration",
                table: "SubCatalog",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "SubCatalog");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "SubCatalog",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "SubCatalog",
                newName: "code");
        }
    }
}
