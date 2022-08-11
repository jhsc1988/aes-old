using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace aes.Migrations
{
    public partial class ApartmentsRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApartmentUpdate");

            migrationBuilder.AlterColumn<decimal>(
                name: "Iznos",
                table: "RacunHolding",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Iznos",
                table: "RacunElektraRate",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Iznos",
                table: "RacunElektraIzvrsenjeUsluge",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Iznos",
                table: "RacunElektra",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateTable(
                name: "StanUpdate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateOfData = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBegan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateEnded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExecutedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateComplete = table.Column<bool>(type: "bit", nullable: false),
                    Interrupted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StanUpdate", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StanUpdate");

            migrationBuilder.AlterColumn<decimal>(
                name: "Iznos",
                table: "RacunHolding",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Iznos",
                table: "RacunElektraRate",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Iznos",
                table: "RacunElektraIzvrsenjeUsluge",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Iznos",
                table: "RacunElektra",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.CreateTable(
                name: "StanUpdate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateOfData = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExecutedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Interrupted = table.Column<bool>(type: "bit", nullable: false),
                    UpdateBegan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateComplete = table.Column<bool>(type: "bit", nullable: false),
                    UpdateEnded = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentUpdate", x => x.Id);
                });
        }
    }
}
