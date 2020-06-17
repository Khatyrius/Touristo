using Microsoft.EntityFrameworkCore.Migrations;

namespace TouristoService.Migrations
{
    public partial class FinalMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_attractions_Cities_cityid",
                table: "attractions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_attractions",
                table: "attractions");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "attractions",
                newName: "Attractions");

            migrationBuilder.RenameIndex(
                name: "IX_attractions_cityid",
                table: "Attractions",
                newName: "IX_Attractions_cityid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attractions",
                table: "Attractions",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attractions_Cities_cityid",
                table: "Attractions",
                column: "cityid",
                principalTable: "Cities",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attractions_Cities_cityid",
                table: "Attractions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Attractions",
                table: "Attractions");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "Attractions",
                newName: "attractions");

            migrationBuilder.RenameIndex(
                name: "IX_Attractions_cityid",
                table: "attractions",
                newName: "IX_attractions_cityid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_attractions",
                table: "attractions",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_attractions_Cities_cityid",
                table: "attractions",
                column: "cityid",
                principalTable: "Cities",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
