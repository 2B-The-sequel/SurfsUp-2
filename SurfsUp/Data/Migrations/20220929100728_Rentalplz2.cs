using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfsUp.Data.Migrations
{
    public partial class Rentalplz2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "applicationUserId",
                table: "Board",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Board_applicationUserId",
                table: "Board",
                column: "applicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Board_AspNetUsers_applicationUserId",
                table: "Board",
                column: "applicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Board_AspNetUsers_applicationUserId",
                table: "Board");

            migrationBuilder.DropIndex(
                name: "IX_Board_applicationUserId",
                table: "Board");

            migrationBuilder.DropColumn(
                name: "applicationUserId",
                table: "Board");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
