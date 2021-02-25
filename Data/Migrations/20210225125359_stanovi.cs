using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace aes.Data.Migrations
{
    public partial class stanovi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StanId = table.Column<int>(type: "int", nullable: false),
                    SifraObjekta = table.Column<int>(type: "int", nullable: false),
                    Vrsta = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Adresa = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Kat = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    BrojSTana = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Naselje = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Četvrt = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Površina = table.Column<double>(type: "float", nullable: true),
                    StatusKorištenja = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Korisnik = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Vlasništvo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    DioNekretnine = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    Sektor = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    VrijemeUnosa = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stan", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stan");
        }
    }
}
