using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Infrastructure.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class Alter_table_active_ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivePause_Clients_ClientId",
                table: "ActivePause");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivePause_Clients_ClientId",
                table: "ActivePause",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivePause_Clients_ClientId",
                table: "ActivePause");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivePause_Clients_ClientId",
                table: "ActivePause",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");
        }
    }
}
