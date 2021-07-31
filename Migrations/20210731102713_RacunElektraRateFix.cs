using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace aes.Migrations
{
    public partial class RacunElektraRateFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RacunElektraRate_Dopis_DopisId",
                table: "RacunElektraRate");

            migrationBuilder.AlterColumn<int>(
                name: "DopisId",
                table: "RacunElektraRate",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "RacunElektraRate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatumIzdavanja",
                table: "RacunElektraRate",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsItTemp",
                table: "RacunElektraRate",
                type: "bit",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RacunElektraRate_Dopis_DopisId",
                table: "RacunElektraRate",
                column: "DopisId",
                principalTable: "Dopis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RacunElektraRate_Dopis_DopisId",
                table: "RacunElektraRate");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "RacunElektraRate");

            migrationBuilder.DropColumn(
                name: "DatumIzdavanja",
                table: "RacunElektraRate");

            migrationBuilder.DropColumn(
                name: "IsItTemp",
                table: "RacunElektraRate");

            migrationBuilder.AlterColumn<int>(
                name: "DopisId",
                table: "RacunElektraRate",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RacunElektraRate_Dopis_DopisId",
                table: "RacunElektraRate",
                column: "DopisId",
                principalTable: "Dopis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
