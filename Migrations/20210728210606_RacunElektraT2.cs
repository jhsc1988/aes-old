using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace aes.Migrations
{
    public partial class RacunElektraT2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "RacuniElektraTemp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RacuniElektraTemp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RacuniElektraTemp_RacunElektra_Id",
                        column: x => x.Id,
                        principalTable: "RacunElektra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RacuniElektraTemp");

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
    }
}
