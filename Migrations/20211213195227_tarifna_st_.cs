using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace aes.Migrations
{
    public partial class tarifna_st_ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TarifnaStavka",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TarifnaStavka", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObracunPotrosnje",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RacunElektraId = table.Column<int>(type: "int", nullable: false),
                    BrojBrojila = table.Column<long>(type: "bigint", nullable: false),
                    TarifnaStavkaId = table.Column<int>(type: "int", nullable: false),
                    DatumOd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumDo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StanjeOd = table.Column<double>(type: "float", nullable: false),
                    StanjeDo = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObracunPotrosnje", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObracunPotrosnje_RacunElektra_RacunElektraId",
                        column: x => x.RacunElektraId,
                        principalTable: "RacunElektra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObracunPotrosnje_TarifnaStavka_TarifnaStavkaId",
                        column: x => x.TarifnaStavkaId,
                        principalTable: "TarifnaStavka",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ObracunPotrosnje_RacunElektraId",
                table: "ObracunPotrosnje",
                column: "RacunElektraId");

            migrationBuilder.CreateIndex(
                name: "IX_ObracunPotrosnje_TarifnaStavkaId",
                table: "ObracunPotrosnje",
                column: "TarifnaStavkaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ObracunPotrosnje");

            migrationBuilder.DropTable(
                name: "TarifnaStavka");
        }
    }
}
