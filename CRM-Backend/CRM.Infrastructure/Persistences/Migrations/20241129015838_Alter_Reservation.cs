using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Infrastructure.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Reservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Reservations",
                newName: "Message");

            migrationBuilder.AddColumn<DateTime>(
                name: "Apointment",
                table: "Reservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Reservations",
                type: "varchar(1000)",
                unicode: false,
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Payment",
                table: "Reservations",
                type: "varchar(1000)",
                unicode: false,
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<short>(
                name: "Price",
                table: "Reservations",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "Tittle",
                table: "Reservations",
                type: "varchar(1000)",
                unicode: false,
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "userBarberId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ClientId",
                table: "Reservations",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_userBarberId",
                table: "Reservations",
                column: "userBarberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Clients_ClientId",
                table: "Reservations",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Users_userBarberId",
                table: "Reservations",
                column: "userBarberId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Clients_ClientId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Users_userBarberId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_ClientId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_userBarberId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Apointment",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Payment",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Tittle",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "userBarberId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "Reservations",
                newName: "Phone");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
