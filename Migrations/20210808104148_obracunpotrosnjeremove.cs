using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace aes.Migrations
{
    public partial class obracunpotrosnjeremove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RacunElektraObracunPotrosnje");

            migrationBuilder.DropTable(
                name: "RacunOdsIzvrsenjaUsluge");

            migrationBuilder.DropTable(
                name: "OdsKupac");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OdsKupac",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Napomena = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    OdsId = table.Column<int>(type: "int", nullable: false),
                    SifraKupca = table.Column<int>(type: "int", nullable: false),
                    VrijemeUnosa = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdsKupac", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OdsKupac_Ods_OdsId",
                        column: x => x.OdsId,
                        principalTable: "Ods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RacunElektraObracunPotrosnje",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatumObracuna = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RNT = table.Column<int>(type: "int", nullable: false),
                    RVT = table.Column<int>(type: "int", nullable: false),
                    RacunElektraId = table.Column<int>(type: "int", nullable: false),
                    VrijemeUnosa = table.Column<DateTime>(type: "datetime2", nullable: true),
                    brojilo = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RacunElektraObracunPotrosnje", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RacunElektraObracunPotrosnje_RacunElektra_RacunElektraId",
                        column: x => x.RacunElektraId,
                        principalTable: "RacunElektra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RacunOdsIzvrsenjaUsluge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojRacuna = table.Column<string>(type: "nvarchar(19)", maxLength: 19, nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatumIzdavanja = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DatumIzvrsenja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumPotvrde = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DopisId = table.Column<int>(type: "int", nullable: true),
                    IsItTemp = table.Column<bool>(type: "bit", nullable: true),
                    Iznos = table.Column<double>(type: "float", nullable: false),
                    KlasaPlacanja = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Napomena = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    OdsKupacId = table.Column<int>(type: "int", nullable: false),
                    RedniBroj = table.Column<int>(type: "int", nullable: false),
                    Usluga = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    VrijemeUnosa = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RacunOdsIzvrsenjaUsluge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RacunOdsIzvrsenjaUsluge_Dopis_DopisId",
                        column: x => x.DopisId,
                        principalTable: "Dopis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RacunOdsIzvrsenjaUsluge_OdsKupac_OdsKupacId",
                        column: x => x.OdsKupacId,
                        principalTable: "OdsKupac",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OdsKupac_OdsId",
                table: "OdsKupac",
                column: "OdsId");

            migrationBuilder.CreateIndex(
                name: "IX_RacunElektraObracunPotrosnje_RacunElektraId",
                table: "RacunElektraObracunPotrosnje",
                column: "RacunElektraId");

            migrationBuilder.CreateIndex(
                name: "IX_RacunOdsIzvrsenjaUsluge_DopisId",
                table: "RacunOdsIzvrsenjaUsluge",
                column: "DopisId");

            migrationBuilder.CreateIndex(
                name: "IX_RacunOdsIzvrsenjaUsluge_OdsKupacId",
                table: "RacunOdsIzvrsenjaUsluge",
                column: "OdsKupacId");
        }
    }
}
