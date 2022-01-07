using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace aes.Migrations
{
    public partial class kupac_edit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ElektraKupacEdit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElektraKupacId = table.Column<int>(type: "int", nullable: true),
                    EditingByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EditTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElektraKupacEdit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElektraKupacEdit_ElektraKupac_ElektraKupacId",
                        column: x => x.ElektraKupacId,
                        principalTable: "ElektraKupac",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ElektraKupacEdit_ElektraKupacId",
                table: "ElektraKupacEdit",
                column: "ElektraKupacId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ElektraKupacEdit");
        }
    }
}
