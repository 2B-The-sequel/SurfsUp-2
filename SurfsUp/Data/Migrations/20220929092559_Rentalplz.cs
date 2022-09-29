using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfsUp.Data.Migrations
{
    public partial class Rentalplz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rental_AspNetUsers_UserId",
                table: "Rental");

            migrationBuilder.DropIndex(
                name: "IX_Rental_BoardId",
                table: "Rental");

            migrationBuilder.DropIndex(
                name: "IX_Rental_UserId",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Rental");

            migrationBuilder.AlterColumn<string>(
                name: "UsersId",
                table: "Rental",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Rental_BoardId",
                table: "Rental",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Rental_UsersId",
                table: "Rental",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_AspNetUsers_UsersId",
                table: "Rental",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rental_AspNetUsers_UsersId",
                table: "Rental");

            migrationBuilder.DropIndex(
                name: "IX_Rental_BoardId",
                table: "Rental");

            migrationBuilder.DropIndex(
                name: "IX_Rental_UsersId",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UsersId",
                table: "Rental",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Rental",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rental_BoardId",
                table: "Rental",
                column: "BoardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rental_UserId",
                table: "Rental",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_AspNetUsers_UserId",
                table: "Rental",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
