using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace aes.Data.Migrations
{
    public partial class racunodsizvrsenjausluge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RacunOdsIzvrsenjaUsluge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojRacuna = table.Column<string>(type: "nvarchar(19)", maxLength: 19, nullable: false),
                    OdsKupacId = table.Column<int>(type: "int", nullable: false),
                    DatumIzdavanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumIzvrsenja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Usluga = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Iznos = table.Column<double>(type: "float", nullable: false),
                    DopisId = table.Column<int>(type: "int", nullable: false),
                    RedniBroj = table.Column<int>(type: "int", nullable: false),
                    KlasaPlacanja = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DatumPotvrde = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RacunOdsIzvrsenjaUsluge_OdsKupac_OdsKupacId",
                        column: x => x.OdsKupacId,
                        principalTable: "OdsKupac",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RacunOdsIzvrsenjaUsluge_DopisId",
                table: "RacunOdsIzvrsenjaUsluge",
                column: "DopisId");

            migrationBuilder.CreateIndex(
                name: "IX_RacunOdsIzvrsenjaUsluge_OdsKupacId",
                table: "RacunOdsIzvrsenjaUsluge",
                column: "OdsKupacId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RacunOdsIzvrsenjaUsluge");
        }
    }
}
