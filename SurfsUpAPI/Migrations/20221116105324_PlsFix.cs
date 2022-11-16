using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfsUpAPI.Migrations
{
    public partial class PlsFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rental_ApplicationUser_UserId",
                table: "Rental");

            migrationBuilder.DropForeignKey(
                name: "FK_Rental_Board_BoardId",
                table: "Rental");

            migrationBuilder.DropIndex(
                name: "IX_Rental_BoardId",
                table: "Rental");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Rental",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Rental_UserId",
                table: "Rental",
                newName: "IX_Rental_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_ApplicationUser_ApplicationUserId",
                table: "Rental",
                column: "ApplicationUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rental_ApplicationUser_ApplicationUserId",
                table: "Rental");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Rental",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Rental_ApplicationUserId",
                table: "Rental",
                newName: "IX_Rental_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rental_BoardId",
                table: "Rental",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_ApplicationUser_UserId",
                table: "Rental",
                column: "UserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_Board_BoardId",
                table: "Rental",
                column: "BoardId",
                principalTable: "Board",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
