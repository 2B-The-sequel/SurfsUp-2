using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfsUpAPI.Migrations
{
    public partial class Rental_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RentalID",
                table: "Rental",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Rental",
                newName: "RentalID");
        }
    }
}
