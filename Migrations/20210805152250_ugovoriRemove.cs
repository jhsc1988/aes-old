using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace aes.Migrations
{
    public partial class ugovoriRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UgovorOPrijenosu");

            migrationBuilder.DropTable(
                name: "UgovorOKoristenju");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UgovorOKoristenju",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojUgovora = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    DatumPotpisaGZ = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumPotpisaHEP = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DopisDostaveId = table.Column<int>(type: "int", nullable: false),
                    DopisId = table.Column<int>(type: "int", nullable: false),
                    OdsId = table.Column<int>(type: "int", nullable: false),
                    RbrDostave = table.Column<int>(type: "int", nullable: false),
                    RbrUgovora = table.Column<int>(type: "int", nullable: false),
                    VrijemeUnosa = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UgovorOKoristenju", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UgovorOKoristenju_Dopis_DopisDostaveId",
                        column: x => x.DopisDostaveId,
                        principalTable: "Dopis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UgovorOKoristenju_Dopis_DopisId",
                        column: x => x.DopisId,
                        principalTable: "Dopis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UgovorOKoristenju_Ods_OdsId",
                        column: x => x.OdsId,
                        principalTable: "Ods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UgovorOPrijenosu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojUgovora = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    DatumPotpisa = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumPrijenosa = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DopisDostaveId = table.Column<int>(type: "int", nullable: false),
                    DopisId = table.Column<int>(type: "int", nullable: false),
                    Kupac = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    KupacOIB = table.Column<long>(type: "bigint", nullable: false),
                    RbrDostave = table.Column<int>(type: "int", nullable: false),
                    RbrUgovora = table.Column<int>(type: "int", nullable: false),
                    UgovorOKoristenjuId = table.Column<int>(type: "int", nullable: false),
                    VrijemeUnosa = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UgovorOPrijenosu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UgovorOPrijenosu_Dopis_DopisDostaveId",
                        column: x => x.DopisDostaveId,
                        principalTable: "Dopis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UgovorOPrijenosu_Dopis_DopisId",
                        column: x => x.DopisId,
                        principalTable: "Dopis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UgovorOPrijenosu_UgovorOKoristenju_UgovorOKoristenjuId",
                        column: x => x.UgovorOKoristenjuId,
                        principalTable: "UgovorOKoristenju",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UgovorOKoristenju_DopisDostaveId",
                table: "UgovorOKoristenju",
                column: "DopisDostaveId");

            migrationBuilder.CreateIndex(
                name: "IX_UgovorOKoristenju_DopisId",
                table: "UgovorOKoristenju",
                column: "DopisId");

            migrationBuilder.CreateIndex(
                name: "IX_UgovorOKoristenju_OdsId",
                table: "UgovorOKoristenju",
                column: "OdsId");

            migrationBuilder.CreateIndex(
                name: "IX_UgovorOPrijenosu_DopisDostaveId",
                table: "UgovorOPrijenosu",
                column: "DopisDostaveId");

            migrationBuilder.CreateIndex(
                name: "IX_UgovorOPrijenosu_DopisId",
                table: "UgovorOPrijenosu",
                column: "DopisId");

            migrationBuilder.CreateIndex(
                name: "IX_UgovorOPrijenosu_UgovorOKoristenjuId",
                table: "UgovorOPrijenosu",
                column: "UgovorOKoristenjuId");
        }
    }
}
