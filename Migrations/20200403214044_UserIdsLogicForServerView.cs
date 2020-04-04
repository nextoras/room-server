using Microsoft.EntityFrameworkCore.Migrations;

namespace sketch_mar21a.Migrations
{
    public partial class UserIdsLogicForServerView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Meterings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Devices",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Meterings_UserId",
                table: "Meterings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meterings_Users_UserId",
                table: "Meterings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meterings_Users_UserId",
                table: "Meterings");

            migrationBuilder.DropIndex(
                name: "IX_Meterings_UserId",
                table: "Meterings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Meterings");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Devices");
        }
    }
}
