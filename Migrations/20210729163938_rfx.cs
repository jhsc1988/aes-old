using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace aes.Migrations
{
    public partial class rfx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RacunElektra_Dopis_DopisId",
                table: "RacunElektra");

            migrationBuilder.AlterColumn<int>(
                name: "DopisId",
                table: "RacunElektra",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DatumIzdavanja",
                table: "RacunElektra",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_RacunElektra_Dopis_DopisId",
                table: "RacunElektra",
                column: "DopisId",
                principalTable: "Dopis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RacunElektra_Dopis_DopisId",
                table: "RacunElektra");

            migrationBuilder.AlterColumn<int>(
                name: "DopisId",
                table: "RacunElektra",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DatumIzdavanja",
                table: "RacunElektra",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RacunElektra_Dopis_DopisId",
                table: "RacunElektra",
                column: "DopisId",
                principalTable: "Dopis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
