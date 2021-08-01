using Microsoft.EntityFrameworkCore.Migrations;

namespace aes.Migrations
{
    public partial class RacunElektra_ElektraKupacNullEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RacunElektra_ElektraKupac_ElektraKupacId",
                table: "RacunElektra");

            migrationBuilder.AlterColumn<int>(
                name: "ElektraKupacId",
                table: "RacunElektra",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_RacunElektra_ElektraKupac_ElektraKupacId",
                table: "RacunElektra",
                column: "ElektraKupacId",
                principalTable: "ElektraKupac",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RacunElektra_ElektraKupac_ElektraKupacId",
                table: "RacunElektra");

            migrationBuilder.AlterColumn<int>(
                name: "ElektraKupacId",
                table: "RacunElektra",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RacunElektra_ElektraKupac_ElektraKupacId",
                table: "RacunElektra",
                column: "ElektraKupacId",
                principalTable: "ElektraKupac",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
