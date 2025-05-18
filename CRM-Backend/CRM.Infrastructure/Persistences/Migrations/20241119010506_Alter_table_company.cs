using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Infrastructure.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class Alter_table_company : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Company_CompanyId",
                table: "Clients");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Company_CompanyId",
                table: "Clients",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Company_CompanyId",
                table: "Clients");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Company_CompanyId",
                table: "Clients",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");
        }
    }
}
