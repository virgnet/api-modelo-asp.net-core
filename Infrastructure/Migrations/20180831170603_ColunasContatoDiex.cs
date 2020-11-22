using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class ColunasContatoDiex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "ContatoDiex",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "ContatoDiex",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Origem",
                table: "ContatoDiex",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "ContatoDiex");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "ContatoDiex");

            migrationBuilder.DropColumn(
                name: "Origem",
                table: "ContatoDiex");
        }
    }
}
