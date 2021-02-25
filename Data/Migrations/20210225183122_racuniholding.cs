using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace aes.Data.Migrations
{
    public partial class racuniholding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RacunHolding",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojRacuna = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StanId = table.Column<int>(type: "int", nullable: false),
                    DatumIzdavanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Iznos = table.Column<double>(type: "float", nullable: false),
                    DopisId = table.Column<int>(type: "int", nullable: false),
                    RedniBroj = table.Column<int>(type: "int", nullable: false),
                    KlasaPlacanja = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DatumPotvrde = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VrijemeUnosa = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RacunHolding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RacunHolding_Dopis_DopisId",
                        column: x => x.DopisId,
                        principalTable: "Dopis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RacunHolding_Stan_StanId",
                        column: x => x.StanId,
                        principalTable: "Stan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RacunHolding_DopisId",
                table: "RacunHolding",
                column: "DopisId");

            migrationBuilder.CreateIndex(
                name: "IX_RacunHolding_StanId",
                table: "RacunHolding",
                column: "StanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RacunHolding");
        }
    }
}
