using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace aes.Data.Migrations
{
    public partial class racunelektraizvrsanjeusluge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RacunElektraIzvrsenjeUsluge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojRacuna = table.Column<string>(type: "nvarchar(19)", maxLength: 19, nullable: false),
                    ElektraKupacId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_RacunElektraIzvrsenjeUsluge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RacunElektraIzvrsenjeUsluge_Dopis_DopisId",
                        column: x => x.DopisId,
                        principalTable: "Dopis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RacunElektraIzvrsenjeUsluge_ElektraKupac_ElektraKupacId",
                        column: x => x.ElektraKupacId,
                        principalTable: "ElektraKupac",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RacunElektraIzvrsenjeUsluge_DopisId",
                table: "RacunElektraIzvrsenjeUsluge",
                column: "DopisId");

            migrationBuilder.CreateIndex(
                name: "IX_RacunElektraIzvrsenjeUsluge_ElektraKupacId",
                table: "RacunElektraIzvrsenjeUsluge",
                column: "ElektraKupacId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RacunElektraIzvrsenjeUsluge");
        }
    }
}
