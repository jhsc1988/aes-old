using Microsoft.EntityFrameworkCore.Migrations;

namespace aes.Data.Migrations
{
    public partial class racuniElektraIzvrsenjeFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Napomena",
                table: "RacunElektraIzvrsenjeUsluge",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Napomena",
                table: "RacunElektraIzvrsenjeUsluge");
        }
    }
}
