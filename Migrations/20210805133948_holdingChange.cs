using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace aes.Migrations
{
    public partial class holdingChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RacunHolding_Dopis_DopisId",
                table: "RacunHolding");

            migrationBuilder.DropForeignKey(
                name: "FK_RacunOdsIzvrsenjaUsluge_Dopis_DopisId",
                table: "RacunOdsIzvrsenjaUsluge");

            migrationBuilder.AlterColumn<int>(
                name: "DopisId",
                table: "RacunOdsIzvrsenjaUsluge",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DatumIzdavanja",
                table: "RacunOdsIzvrsenjaUsluge",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "RacunOdsIzvrsenjaUsluge",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsItTemp",
                table: "RacunOdsIzvrsenjaUsluge",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DopisId",
                table: "RacunHolding",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DatumIzdavanja",
                table: "RacunHolding",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "BrojRacuna",
                table: "RacunHolding",
                type: "nvarchar(19)",
                maxLength: 19,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "RacunHolding",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsItTemp",
                table: "RacunHolding",
                type: "bit",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RacunHolding_Dopis_DopisId",
                table: "RacunHolding",
                column: "DopisId",
                principalTable: "Dopis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RacunOdsIzvrsenjaUsluge_Dopis_DopisId",
                table: "RacunOdsIzvrsenjaUsluge",
                column: "DopisId",
                principalTable: "Dopis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RacunHolding_Dopis_DopisId",
                table: "RacunHolding");

            migrationBuilder.DropForeignKey(
                name: "FK_RacunOdsIzvrsenjaUsluge_Dopis_DopisId",
                table: "RacunOdsIzvrsenjaUsluge");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "RacunOdsIzvrsenjaUsluge");

            migrationBuilder.DropColumn(
                name: "IsItTemp",
                table: "RacunOdsIzvrsenjaUsluge");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "RacunHolding");

            migrationBuilder.DropColumn(
                name: "IsItTemp",
                table: "RacunHolding");

            migrationBuilder.AlterColumn<int>(
                name: "DopisId",
                table: "RacunOdsIzvrsenjaUsluge",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DatumIzdavanja",
                table: "RacunOdsIzvrsenjaUsluge",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DopisId",
                table: "RacunHolding",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DatumIzdavanja",
                table: "RacunHolding",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BrojRacuna",
                table: "RacunHolding",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(19)",
                oldMaxLength: 19);

            migrationBuilder.AddForeignKey(
                name: "FK_RacunHolding_Dopis_DopisId",
                table: "RacunHolding",
                column: "DopisId",
                principalTable: "Dopis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RacunOdsIzvrsenjaUsluge_Dopis_DopisId",
                table: "RacunOdsIzvrsenjaUsluge",
                column: "DopisId",
                principalTable: "Dopis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
