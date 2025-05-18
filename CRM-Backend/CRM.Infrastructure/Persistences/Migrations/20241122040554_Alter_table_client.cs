using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Infrastructure.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class Alter_table_client : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "ActivePause",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ActivePause_ClientId",
                table: "ActivePause",
                column: "ClientId");

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

            migrationBuilder.DropIndex(
                name: "IX_ActivePause_ClientId",
                table: "ActivePause");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "ActivePause");
        }
    }
}
