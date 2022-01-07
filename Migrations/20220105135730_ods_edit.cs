using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace aes.Migrations
{
    public partial class ods_edit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEditing",
                table: "Ods",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "OdsEdit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OdsId = table.Column<int>(type: "int", nullable: true),
                    EditingByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EditTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdsEdit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OdsEdit_Ods_OdsId",
                        column: x => x.OdsId,
                        principalTable: "Ods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OdsEdit_OdsId",
                table: "OdsEdit",
                column: "OdsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OdsEdit");

            migrationBuilder.DropColumn(
                name: "IsEditing",
                table: "Ods");
        }
    }
}
