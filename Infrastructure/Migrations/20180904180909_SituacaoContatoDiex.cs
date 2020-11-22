using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class SituacaoContatoDiex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "IdSituacao",
                table: "ContatoDiex",
                nullable: false,
                defaultValue: new Guid("6EF5DCCF-3208-48D0-845D-4A4A2C0A9646"));

            migrationBuilder.CreateIndex(
                name: "IX_ContatoDiex_IdSituacao",
                table: "ContatoDiex",
                column: "IdSituacao");

            migrationBuilder.AddForeignKey(
                name: "FK_ContatoDiex_Indicador_IdSituacao",
                table: "ContatoDiex",
                column: "IdSituacao",
                principalTable: "Indicador",
                principalColumn: "IdIndicador",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContatoDiex_Indicador_IdSituacao",
                table: "ContatoDiex");

            migrationBuilder.DropIndex(
                name: "IX_ContatoDiex_IdSituacao",
                table: "ContatoDiex");

            migrationBuilder.DropColumn(
                name: "IdSituacao",
                table: "ContatoDiex");
        }
    }
}
