using Microsoft.EntityFrameworkCore.Migrations;

namespace aes.Migrations
{
    public partial class RacunElektraTempUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Napomena",
                table: "RacunElektraTemp",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Napomena",
                table: "RacunElektraTemp");
        }
    }
}
