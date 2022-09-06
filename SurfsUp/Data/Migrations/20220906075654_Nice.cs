using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfsUp.Data.Migrations
{
    public partial class Nice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardEquipment_Board_BoardsId",
                table: "BoardEquipment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BoardEquipment",
                table: "BoardEquipment");

            migrationBuilder.DropIndex(
                name: "IX_BoardEquipment_EquipmentId",
                table: "BoardEquipment");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Equipment",
                newName: "EquipmentId");

            migrationBuilder.RenameColumn(
                name: "BoardsId",
                table: "BoardEquipment",
                newName: "BoardId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Board",
                newName: "BoardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BoardEquipment",
                table: "BoardEquipment",
                columns: new[] { "EquipmentId", "BoardId" });

            migrationBuilder.CreateIndex(
                name: "IX_BoardEquipment_BoardId",
                table: "BoardEquipment",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardEquipment_Board_BoardId",
                table: "BoardEquipment",
                column: "BoardId",
                principalTable: "Board",
                principalColumn: "BoardId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardEquipment_Board_BoardId",
                table: "BoardEquipment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BoardEquipment",
                table: "BoardEquipment");

            migrationBuilder.DropIndex(
                name: "IX_BoardEquipment_BoardId",
                table: "BoardEquipment");

            migrationBuilder.RenameColumn(
                name: "EquipmentId",
                table: "Equipment",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "BoardId",
                table: "BoardEquipment",
                newName: "BoardsId");

            migrationBuilder.RenameColumn(
                name: "BoardId",
                table: "Board",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BoardEquipment",
                table: "BoardEquipment",
                columns: new[] { "BoardsId", "EquipmentId" });

            migrationBuilder.CreateIndex(
                name: "IX_BoardEquipment_EquipmentId",
                table: "BoardEquipment",
                column: "EquipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardEquipment_Board_BoardsId",
                table: "BoardEquipment",
                column: "BoardsId",
                principalTable: "Board",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
