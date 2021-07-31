using Microsoft.EntityFrameworkCore.Migrations;

namespace aes.Migrations
{
    public partial class Racun_add_created_by : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "RacunElektra",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "RacunElektra");
        }
    }
}
