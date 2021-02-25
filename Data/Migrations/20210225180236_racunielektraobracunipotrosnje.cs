using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace aes.Data.Migrations
{
    public partial class racunielektraobracunipotrosnje : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RacunElektraObracunPotrosnje",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatumObracuna = table.Column<DateTime>(type: "datetime2", nullable: false),
                    brojilo = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    BrojRacuna = table.Column<int>(type: "int", nullable: false),
                    RVT = table.Column<int>(type: "int", nullable: false),
                    RNT = table.Column<int>(type: "int", nullable: false),
                    VrijemeUnosa = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RacunElektraObracunPotrosnje", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RacunElektraObracunPotrosnje");
        }
    }
}
