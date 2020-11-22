using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class temaUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Tema",
                type: "varchar(MAX)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Imagem",
                table: "Tema",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Tema");

            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "Tema");
        }
    }
}
