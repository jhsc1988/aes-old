using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace aes.Migrations
{
    public partial class edits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BrojRacuna",
                table: "RacunHolding",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(21)",
                oldMaxLength: 21);

            migrationBuilder.CreateTable(
                name: "RacunElektraEdit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RacunElektraId = table.Column<int>(type: "int", nullable: false),
                    EditingByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EditTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RacunElektraEdit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RacunElektraEdit_RacunElektra_RacunElektraId",
                        column: x => x.RacunElektraId,
                        principalTable: "RacunElektra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RacunElektraIzvrsenjeUslugeEdit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RacunElektraIzvrsenjeUslugeId = table.Column<int>(type: "int", nullable: false),
                    EditingByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EditTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RacunElektraIzvrsenjeUslugeEdit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RacunElektraIzvrsenjeUslugeEdit_RacunElektraIzvrsenjeUsluge_RacunElektraIzvrsenjeUslugeId",
                        column: x => x.RacunElektraIzvrsenjeUslugeId,
                        principalTable: "RacunElektraIzvrsenjeUsluge",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RacunElektraRateEdit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RacunElektraRateId = table.Column<int>(type: "int", nullable: false),
                    EditingByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EditTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RacunElektraRateEdit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RacunElektraRateEdit_RacunElektraRate_RacunElektraRateId",
                        column: x => x.RacunElektraRateId,
                        principalTable: "RacunElektraRate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RacunElektraEdit_RacunElektraId",
                table: "RacunElektraEdit",
                column: "RacunElektraId");

            migrationBuilder.CreateIndex(
                name: "IX_RacunElektraIzvrsenjeUslugeEdit_RacunElektraIzvrsenjeUslugeId",
                table: "RacunElektraIzvrsenjeUslugeEdit",
                column: "RacunElektraIzvrsenjeUslugeId");

            migrationBuilder.CreateIndex(
                name: "IX_RacunElektraRateEdit_RacunElektraRateId",
                table: "RacunElektraRateEdit",
                column: "RacunElektraRateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RacunElektraEdit");

            migrationBuilder.DropTable(
                name: "RacunElektraIzvrsenjeUslugeEdit");

            migrationBuilder.DropTable(
                name: "RacunElektraRateEdit");

            migrationBuilder.AlterColumn<string>(
                name: "BrojRacuna",
                table: "RacunHolding",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);
        }
    }
}
