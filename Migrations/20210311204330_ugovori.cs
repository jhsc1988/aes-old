using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace aes.Migrations
{
    public partial class ugovori : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Klasa",
                table: "Predmet",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8);

            migrationBuilder.CreateTable(
                name: "UgovorOKoristenju",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojUgovora = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    OdsId = table.Column<int>(type: "int", nullable: false),
                    DatumPotpisaHEP = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumPotpisaGZ = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DopisId = table.Column<int>(type: "int", nullable: false),
                    RbrUgovora = table.Column<int>(type: "int", nullable: false),
                    DopisDostaveId = table.Column<int>(type: "int", nullable: false),
                    RbrDostave = table.Column<int>(type: "int", nullable: false),
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
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UgovorOKoristenju_Dopis_DopisId",
                        column: x => x.DopisId,
                        principalTable: "Dopis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UgovorOKoristenju_Ods_OdsId",
                        column: x => x.OdsId,
                        principalTable: "Ods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UgovorOPrijenosu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojUgovora = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    UgovorOKoristenjuId = table.Column<int>(type: "int", nullable: false),
                    DatumPrijenosa = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumPotpisa = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Kupac = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    KupacOIB = table.Column<long>(type: "bigint", nullable: false),
                    DopisId = table.Column<int>(type: "int", nullable: false),
                    RbrUgovora = table.Column<int>(type: "int", nullable: false),
                    DopisDostaveId = table.Column<int>(type: "int", nullable: false),
                    RbrDostave = table.Column<int>(type: "int", nullable: false),
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
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UgovorOPrijenosu_Dopis_DopisId",
                        column: x => x.DopisId,
                        principalTable: "Dopis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UgovorOPrijenosu_UgovorOKoristenju_UgovorOKoristenjuId",
                        column: x => x.UgovorOKoristenjuId,
                        principalTable: "UgovorOKoristenju",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UgovorOPrijenosu");

            migrationBuilder.DropTable(
                name: "UgovorOKoristenju");

            migrationBuilder.AlterColumn<string>(
                name: "Klasa",
                table: "Predmet",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(21)",
                oldMaxLength: 21);
        }
    }
}
