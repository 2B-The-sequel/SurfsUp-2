using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfsUp.Data.Migrations
{
    public partial class Equipment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_Board_BoardId",
                table: "Equipment");

            migrationBuilder.DropIndex(
                name: "IX_Equipment_BoardId",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "Equipment");

            migrationBuilder.CreateTable(
                name: "BoardEquipment",
                columns: table => new
                {
                    BoardsId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardEquipment", x => new { x.BoardsId, x.EquipmentId });
                    table.ForeignKey(
                        name: "FK_BoardEquipment_Board_BoardsId",
                        column: x => x.BoardsId,
                        principalTable: "Board",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoardEquipment_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardEquipment_EquipmentId",
                table: "BoardEquipment",
                column: "EquipmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardEquipment");

            migrationBuilder.AddColumn<int>(
                name: "BoardId",
                table: "Equipment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_BoardId",
                table: "Equipment",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipment_Board_BoardId",
                table: "Equipment",
                column: "BoardId",
                principalTable: "Board",
                principalColumn: "Id");
        }
    }
}
