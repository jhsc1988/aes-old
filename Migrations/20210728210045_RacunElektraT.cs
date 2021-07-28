using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace aes.Migrations
{
    public partial class RacunElektraT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RacunElektraT");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "RacunElektra",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "RacunElektra",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "RacunElektra",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "RacunElektra");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "RacunElektra");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RacunElektra");

            migrationBuilder.CreateTable(
                name: "RacunElektraT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojRacuna = table.Column<string>(type: "nvarchar(19)", maxLength: 19, nullable: false),
                    DatumIzdavanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumPotvrde = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DopisId = table.Column<int>(type: "int", nullable: false),
                    ElektraKupacId = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Iznos = table.Column<double>(type: "float", nullable: false),
                    KlasaPlacanja = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Napomena = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RedniBroj = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VrijemeUnosa = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RacunElektraT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RacunElektraT_Dopis_DopisId",
                        column: x => x.DopisId,
                        principalTable: "Dopis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RacunElektraT_ElektraKupac_ElektraKupacId",
                        column: x => x.ElektraKupacId,
                        principalTable: "ElektraKupac",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RacunElektraT_DopisId",
                table: "RacunElektraT",
                column: "DopisId");

            migrationBuilder.CreateIndex(
                name: "IX_RacunElektraT_ElektraKupacId",
                table: "RacunElektraT",
                column: "ElektraKupacId");
        }
    }
}
