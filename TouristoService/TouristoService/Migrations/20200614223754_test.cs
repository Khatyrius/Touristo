using Microsoft.EntityFrameworkCore.Migrations;

namespace TouristoService.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Countires_countryid",
                table: "Cities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countires",
                table: "Countires");

            migrationBuilder.RenameTable(
                name: "Countires",
                newName: "Countries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                table: "Countries",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Countries_countryid",
                table: "Cities",
                column: "countryid",
                principalTable: "Countries",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Countries_countryid",
                table: "Cities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                table: "Countries");

            migrationBuilder.RenameTable(
                name: "Countries",
                newName: "Countires");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countires",
                table: "Countires",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Countires_countryid",
                table: "Cities",
                column: "countryid",
                principalTable: "Countires",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
