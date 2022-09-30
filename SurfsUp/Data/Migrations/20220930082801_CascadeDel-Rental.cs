using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfsUp.Data.Migrations
{
    public partial class CascadeDelRental : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rental_AspNetUsers_UsersId",
                table: "Rental");

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_AspNetUsers_UsersId",
                table: "Rental",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rental_AspNetUsers_UsersId",
                table: "Rental");

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_AspNetUsers_UsersId",
                table: "Rental",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
