using Microsoft.EntityFrameworkCore.Migrations;

namespace TouristoService.Migrations
{
    public partial class UpdateCountriesData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "Countires",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "Countires");
        }
    }
}
