using Microsoft.EntityFrameworkCore.Migrations;

namespace aes.Data.Migrations
{
    public partial class ugovorniRacunFromIntToLong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "UgovorniRacun",
                table: "ElektraKupac",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UgovorniRacun",
                table: "ElektraKupac",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
