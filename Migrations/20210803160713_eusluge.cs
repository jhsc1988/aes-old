using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace aes.Migrations
{
    public partial class eusluge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RacunElektraIzvrsenjeUsluge_Dopis_DopisId",
                table: "RacunElektraIzvrsenjeUsluge");

            migrationBuilder.AlterColumn<int>(
                name: "DopisId",
                table: "RacunElektraIzvrsenjeUsluge",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DatumIzdavanja",
                table: "RacunElektraIzvrsenjeUsluge",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "RacunElektraIzvrsenjeUsluge",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsItTemp",
                table: "RacunElektraIzvrsenjeUsluge",
                type: "bit",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RacunElektraIzvrsenjeUsluge_Dopis_DopisId",
                table: "RacunElektraIzvrsenjeUsluge",
                column: "DopisId",
                principalTable: "Dopis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RacunElektraIzvrsenjeUsluge_Dopis_DopisId",
                table: "RacunElektraIzvrsenjeUsluge");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "RacunElektraIzvrsenjeUsluge");

            migrationBuilder.DropColumn(
                name: "IsItTemp",
                table: "RacunElektraIzvrsenjeUsluge");

            migrationBuilder.AlterColumn<int>(
                name: "DopisId",
                table: "RacunElektraIzvrsenjeUsluge",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DatumIzdavanja",
                table: "RacunElektraIzvrsenjeUsluge",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RacunElektraIzvrsenjeUsluge_Dopis_DopisId",
                table: "RacunElektraIzvrsenjeUsluge",
                column: "DopisId",
                principalTable: "Dopis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
