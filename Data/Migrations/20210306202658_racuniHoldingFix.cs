﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace aes.Data.Migrations
{
    public partial class racuniHoldingFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Napomena",
                table: "RacunHolding",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Napomena",
                table: "RacunHolding");
        }
    }
}
