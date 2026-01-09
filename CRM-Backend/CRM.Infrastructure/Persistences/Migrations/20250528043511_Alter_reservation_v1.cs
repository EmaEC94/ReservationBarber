using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Infrastructure.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class Alter_reservation_v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Users_userBarberId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "userBarberId",
                table: "Reservations",
                newName: "UserBarberId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_userBarberId",
                table: "Reservations",
                newName: "IX_Reservations_UserBarberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Users_UserBarberId",
                table: "Reservations",
                column: "UserBarberId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Users_UserBarberId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "UserBarberId",
                table: "Reservations",
                newName: "userBarberId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_UserBarberId",
                table: "Reservations",
                newName: "IX_Reservations_userBarberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Users_userBarberId",
                table: "Reservations",
                column: "userBarberId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
